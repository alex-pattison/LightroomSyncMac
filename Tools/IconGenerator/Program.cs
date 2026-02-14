// Generates app_icon.ico for embedding in the exe (taskbar pinning, shortcuts).
// Run: dotnet run --project Tools/IconGenerator
using System.Drawing;
using System.Drawing.Imaging;
using LightroomSync;

var sizes = new[] { 16, 32, 48, 256 };
var pngs = new List<(byte[] Data, int Width, int Height)>();

foreach (var size in sizes)
{
    using var bitmap = SpinningSyncIcon.CreateAppIconBitmapTransparent(ApertureIconState.Error, size);
    using var ms = new MemoryStream();
    bitmap.Save(ms, ImageFormat.Png);
    pngs.Add((ms.ToArray(), size, size));
}

// Output to LightroomSync project dir (relative to repo root when run via dotnet run --project)
var baseDir = Directory.GetCurrentDirectory();
if (baseDir.EndsWith("IconGenerator", StringComparison.OrdinalIgnoreCase) || baseDir.Contains("bin"))
    baseDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
var outPath = Path.Combine(baseDir, "LightroomSync", "app_icon.ico");
var outDir = Path.GetDirectoryName(outPath)!;
Directory.CreateDirectory(outDir);

await using var output = File.Create(outPath);
using var writer = new BinaryWriter(output);

// ICO header
writer.Write((short)0);
writer.Write((short)1);
writer.Write((short)pngs.Count);

long offset = 6 + (16L * pngs.Count);

foreach (var png in pngs)
{
    writer.Write((byte)(png.Width >= 256 ? 0 : png.Width));
    writer.Write((byte)(png.Height >= 256 ? 0 : png.Height));
    writer.Write((byte)0);
    writer.Write((byte)0);
    writer.Write((short)0);
    writer.Write((short)32);
    writer.Write((uint)png.Data.Length);
    writer.Write((uint)offset);
    offset += png.Data.Length;
}

foreach (var png in pngs)
    writer.Write(png.Data);

Console.WriteLine($"Generated {outPath}");
