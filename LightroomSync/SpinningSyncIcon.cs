using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;

namespace LightroomSync
{
    /// <summary>
    /// Visual state of the aperture status icon.
    /// </summary>
    public enum ApertureIconState
    {
        DimIdle,
        Idle,
        Active,
        LightroomOpen,
        Warning,
        Error
    }

    /// <summary>
    /// Displays the aperture icon. Renders to offscreen buffer to avoid clipping; solid background.
    /// Uses time-based animation and motion-blur trail for smooth spinning.
    /// </summary>
    internal class SpinningSyncIcon : Control
    {
        private readonly Stopwatch _spinStopwatch = new();
        private readonly Stopwatch _pulseStopwatch = new();
        private System.Windows.Forms.Timer? _animTimer;
        private ApertureIconState _state = ApertureIconState.DimIdle;
        private float _stoppedAngle;
        private bool _isPulsing;
        private static Image? _baseImage;
        private static Bitmap? _baseImageWithTransparency;

        private const int IconSize = 72;
        private const int PulseDurationMs = 1500;
        private const int PulseIntervalMs = 30;
        private const double SpinDegreesPerSecond = 72;
        private const int SpinIntervalMs = 10;
        private const int MotionBlurTrails = 18;
        private const float TrailStepDeg = 3f;
        private static readonly float[] MotionBlurAlpha = { 0.55f, 0.5f, 0.45f, 0.4f, 0.35f, 0.3f, 0.25f, 0.2f, 0.16f, 0.12f, 0.09f, 0.06f, 0.04f, 0.025f, 0.015f, 0.008f, 0.004f, 0.002f };

        private static readonly Color PanelBg = Color.FromArgb(32, 32, 36);

        public ApertureIconState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                if (_state == ApertureIconState.Active && value != ApertureIconState.Active)
                {
                    _stoppedAngle = GetCurrentAngle();
                    if (value == ApertureIconState.Idle) { _isPulsing = true; _pulseStopwatch.Restart(); }
                }
                else if (_state == ApertureIconState.LightroomOpen && value == ApertureIconState.Idle)
                {
                    _isPulsing = true;
                    _pulseStopwatch.Restart();
                }
                _state = value;
                UpdateAnimation();
                Invalidate();
            }
        }

        public SpinningSyncIcon()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = PanelBg;
            Size = new Size(IconSize, IconSize);
            MinimumSize = new Size(IconSize, IconSize);
            _animTimer = new System.Windows.Forms.Timer { Interval = SpinIntervalMs };
            _animTimer.Tick += (s, e) =>
            {
                if (_isPulsing && _pulseStopwatch.ElapsedMilliseconds >= PulseDurationMs)
                {
                    _isPulsing = false;
                    _animTimer?.Stop();
                }
                Invalidate();
            };
            LoadIcon();
        }

        /// <summary>
        /// Creates a 32bpp bitmap of the aperture with transparent background. For ICO embedding.
        /// Uses pixel-level alpha to make black/near-black background transparent.
        /// </summary>
        public static Bitmap CreateAppIconBitmapTransparent(ApertureIconState state, int size)
        {
            LoadIcon();
            var bitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(Color.Transparent);

                var src = GetBaseImageWithTransparentBackground();
                if (src == null)
                {
                    using var brush = new SolidBrush(Color.FromArgb(80, 80, 85));
                    g.FillEllipse(brush, 2, 2, size - 4, size - 4);
                }
                else
                {
                    var drawSize = (int)(size * 0.7f);
                    var x = (size - drawSize) / 2f;
                    var y = (size - drawSize) / 2f;
                    var srcRect = new Rectangle(0, 0, src.Width, src.Height);
                    var dest = new Rectangle((int)x, (int)y, drawSize, drawSize);

                    using var attrs = CreateAttributesForState(state);
                    if (attrs != null)
                        g.DrawImage(src, dest, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, attrs);
                    else
                        g.DrawImage(src, dest, srcRect, GraphicsUnit.Pixel);
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Returns a copy of the base image with black/near-black pixels made fully transparent.
        /// Cached; BGRA format, so dark pixels (R,G,B all &lt;= 35) get alpha=0.
        /// </summary>
        private static Bitmap? GetBaseImageWithTransparentBackground()
        {
            if (_baseImage == null) return null;
            if (_baseImageWithTransparency != null) return _baseImageWithTransparency;

            var w = _baseImage.Width;
            var h = _baseImage.Height;
            var bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
                g.DrawImage(_baseImage, 0, 0, w, h);

            var rect = new Rectangle(0, 0, w, h);
            var data = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            try
            {
                var bytes = Math.Abs(data.Stride) * data.Height;
                var pixels = new byte[bytes];
                Marshal.Copy(data.Scan0, pixels, 0, bytes);
                const int threshold = 35;
                for (var i = 0; i < pixels.Length; i += 4)
                {
                    var b = pixels[i];
                    var g = pixels[i + 1];
                    var r = pixels[i + 2];
                    if (r <= threshold && g <= threshold && b <= threshold)
                        pixels[i + 3] = 0;
                }
                Marshal.Copy(pixels, 0, data.Scan0, bytes);
            }
            finally
            {
                bmp.UnlockBits(data);
            }

            _baseImageWithTransparency = bmp;
            return bmp;
        }

        /// <summary>
        /// Creates an Icon from the aperture image in the given state. Caller must keep the returned
        /// Bitmap alive for the Icon to remain valid (e.g. store in a field).
        /// </summary>
        public static (Icon icon, Bitmap bitmap) CreateAppIcon(ApertureIconState state, int size = 32)
        {
            var bitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(PanelBg);

                if (_baseImage == null)
                {
                    using var brush = new SolidBrush(Color.FromArgb(80, 80, 85));
                    g.FillEllipse(brush, 2, 2, size - 4, size - 4);
                }
                else
                {
                    var drawSize = (int)(size * 0.7f);
                    var x = (size - drawSize) / 2f;
                    var y = (size - drawSize) / 2f;
                    var src = new Rectangle(0, 0, _baseImage.Width, _baseImage.Height);
                    var dest = new Rectangle((int)x, (int)y, drawSize, drawSize);

                    using var attrs = CreateAttributesForState(state);
                    if (attrs != null)
                        g.DrawImage(_baseImage, dest, src.X, src.Y, src.Width, src.Height, GraphicsUnit.Pixel, attrs);
                    else
                        g.DrawImage(_baseImage, dest, src, GraphicsUnit.Pixel);
                }
            }

            var icon = Icon.FromHandle(bitmap.GetHicon());
            return (icon, bitmap);
        }

        private static ImageAttributes? CreateAttributesForState(ApertureIconState state)
        {
            var attrs = new ImageAttributes();
            switch (state)
            {
                case ApertureIconState.DimIdle:
                    attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                        new float[] { 0.5f, 0, 0, 0, 0 },
                        new float[] { 0, 0.5f, 0, 0, 0 },
                        new float[] { 0, 0, 0.5f, 0, 0 },
                        new float[] { 0, 0, 0, 0.5f, 0 },
                        new float[] { 0, 0, 0, 0, 1 }
                    }));
                    break;
                case ApertureIconState.LightroomOpen:
                    attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                        new float[] { 0.6f, 0, 0, 0, 0 },
                        new float[] { 0, 0.8f, 0, 0, 0 },
                        new float[] { 0, 0, 0.6f, 0, 0 },
                        new float[] { 0, 0, 0, 1f, 0 },
                        new float[] { 0.05f, 0.12f, 0.05f, 0, 1 }
                    }));
                    break;
                case ApertureIconState.Warning:
                    attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                        new float[] { 1f, 0, 0, 0, 0 },
                        new float[] { 0, 1f, 0, 0, 0 },
                        new float[] { 0.1f, 0.1f, 0.4f, 0, 0 },
                        new float[] { 0, 0, 0, 1f, 0 },
                        new float[] { 0.1f, 0.08f, 0, 0, 1 }
                    }));
                    break;
                case ApertureIconState.Error:
                    attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                        new float[] { 1f, 0, 0, 0, 0 },
                        new float[] { 0.4f, 0.2f, 0, 0, 0 },
                        new float[] { 0.4f, 0, 0.2f, 0, 0 },
                        new float[] { 0, 0, 0, 1f, 0 },
                        new float[] { 0.2f, 0, 0, 0, 1 }
                    }));
                    break;
                default:
                    attrs.Dispose();
                    return null;
            }
            return attrs;
        }

        private static void LoadIcon()
        {
            if (_baseImage != null) return;
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                var name = Array.Find(asm.GetManifestResourceNames(), n => n.EndsWith("aperture_icon.png"));
                if (name != null)
                {
                    using var stream = asm.GetManifestResourceStream(name);
                    if (stream != null)
                    {
                        using var loaded = Image.FromStream(stream);
                        _baseImage = new Bitmap(loaded);
                    }
                }
            }
            catch { }
        }

        private void UpdateAnimation()
        {
            if (_animTimer == null) return;
            if (_state == ApertureIconState.Active)
            {
                _spinStopwatch.Restart();
                _animTimer.Interval = SpinIntervalMs;
                _animTimer.Start();
            }
            else if (_isPulsing)
            {
                _animTimer.Interval = PulseIntervalMs;
                _animTimer.Start();
            }
            else
            {
                _animTimer.Stop();
            }
        }

        private float GetCurrentAngle()
        {
            var elapsed = _spinStopwatch.Elapsed.TotalMilliseconds;
            return (float)((elapsed * SpinDegreesPerSecond / 1000.0 + _stoppedAngle) % 360.0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width <= 0 || Height <= 0) return;

            var size = Math.Min(Width, Height);
            if (size <= 0) return;

            // Render to offscreen buffer - guarantees no clipping, clean rotation
            using (var buffer = new Bitmap(size, size, PixelFormat.Format32bppArgb))
            using (var g = Graphics.FromImage(buffer))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(PanelBg);

                if (_baseImage == null)
                {
                    using var brush = new SolidBrush(Color.FromArgb(80, 80, 85));
                    g.FillEllipse(brush, 2, 2, size - 4, size - 4);
                }
                else
                {
                    var drawSize = (int)(size * 0.7f);
                    var x = (size - drawSize) / 2f;
                    var y = (size - drawSize) / 2f;
                    var cx = size / 2f;
                    var cy = size / 2f;
                    var src = new Rectangle(0, 0, _baseImage.Width, _baseImage.Height);
                    var dest = new Rectangle((int)x, (int)y, drawSize, drawSize);

                    if (_state == ApertureIconState.Active)
                    {
                        var angle = GetCurrentAngle();

                        for (var i = MotionBlurTrails - 1; i >= 0; i--)
                        {
                            var trailAngle = angle - (i + 1) * TrailStepDeg;
                            using var trailAttrs = CreateAlphaAttributes(MotionBlurAlpha[i]);
                            g.TranslateTransform(cx, cy);
                            g.RotateTransform(trailAngle);
                            g.TranslateTransform(-cx, -cy);
                            g.DrawImage(_baseImage, dest, src.X, src.Y, src.Width, src.Height, GraphicsUnit.Pixel, trailAttrs);
                            g.ResetTransform();
                        }

                        g.TranslateTransform(cx, cy);
                        g.RotateTransform(angle);
                        g.TranslateTransform(-cx, -cy);
                        g.DrawImage(_baseImage, dest, src, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        ImageAttributes? attrs;
                        if (_state == ApertureIconState.Idle && _isPulsing)
                        {
                            var elapsed = _pulseStopwatch.ElapsedMilliseconds;
                            var intensity = (float)Math.Max(0, 1 - (double)elapsed / PulseDurationMs);
                            attrs = CreateWhitePulseAttributes(intensity);
                        }
                        else
                        {
                            attrs = GetImageAttributes();
                        }
                        g.TranslateTransform(cx, cy);
                        g.RotateTransform(_stoppedAngle);
                        g.TranslateTransform(-cx, -cy);
                        if (attrs != null)
                            g.DrawImage(_baseImage, dest, src.X, src.Y, src.Width, src.Height, GraphicsUnit.Pixel, attrs);
                        else
                            g.DrawImage(_baseImage, dest, src, GraphicsUnit.Pixel);
                        attrs?.Dispose();
                    }
                }

                e.Graphics.DrawImage(buffer, 0, 0);
            }
        }

        private static ImageAttributes CreateWhitePulseAttributes(float intensity)
        {
            var attrs = new ImageAttributes();
            var brighten = 0.6f * intensity;
            attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                new float[] { 1f + brighten, 0, 0, 0, 0 },
                new float[] { 0, 1f + brighten, 0, 0, 0 },
                new float[] { 0, 0, 1f + brighten, 0, 0 },
                new float[] { 0, 0, 0, 1f, 0 },
                new float[] { brighten, brighten, brighten, 0, 1 }
            }));
            return attrs;
        }

        private static ImageAttributes CreateAlphaAttributes(float alpha)
        {
            var attrs = new ImageAttributes();
            attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                new float[] { 1, 0, 0, 0, 0 },
                new float[] { 0, 1, 0, 0, 0 },
                new float[] { 0, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, alpha, 0 },
                new float[] { 0, 0, 0, 0, 1 }
            }));
            return attrs;
        }

        private ImageAttributes? GetImageAttributes()
        {
            var attrs = new ImageAttributes();
            switch (_state)
            {
                case ApertureIconState.DimIdle:
                    attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                        new float[] { 0.5f, 0, 0, 0, 0 },
                        new float[] { 0, 0.5f, 0, 0, 0 },
                        new float[] { 0, 0, 0.5f, 0, 0 },
                        new float[] { 0, 0, 0, 0.5f, 0 },
                        new float[] { 0, 0, 0, 0, 1 }
                    }));
                    break;
                case ApertureIconState.LightroomOpen:
                    attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                        new float[] { 0.6f, 0, 0, 0, 0 },
                        new float[] { 0, 0.8f, 0, 0, 0 },
                        new float[] { 0, 0, 0.6f, 0, 0 },
                        new float[] { 0, 0, 0, 1f, 0 },
                        new float[] { 0.05f, 0.12f, 0.05f, 0, 1 }
                    }));
                    break;
                case ApertureIconState.Warning:
                    attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                        new float[] { 1f, 0, 0, 0, 0 },
                        new float[] { 0, 1f, 0, 0, 0 },
                        new float[] { 0.1f, 0.1f, 0.4f, 0, 0 },
                        new float[] { 0, 0, 0, 1f, 0 },
                        new float[] { 0.1f, 0.08f, 0, 0, 1 }
                    }));
                    break;
                case ApertureIconState.Error:
                    attrs.SetColorMatrix(new ColorMatrix(new float[][] {
                        new float[] { 1f, 0, 0, 0, 0 },
                        new float[] { 0.4f, 0.2f, 0, 0, 0 },
                        new float[] { 0.4f, 0, 0.2f, 0, 0 },
                        new float[] { 0, 0, 0, 1f, 0 },
                        new float[] { 0.2f, 0, 0, 0, 1 }
                    }));
                    break;
                default:
                    attrs.Dispose();
                    return null;
            }
            return attrs;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _animTimer?.Stop();
            _animTimer = null;
            base.Dispose(disposing);
        }
    }
}
