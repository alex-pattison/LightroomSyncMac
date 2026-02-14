using LightroomSync.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Security.Principal;
using static LightroomSync.Alert;
using static System.Net.Mime.MediaTypeNames;

namespace LightroomSync
{
    public partial class Form1 : Form
    {
        public string currentVersion = "1.0.0"; // <----- Make sure you always update latestVersion.txt as well!
                                                // Yes, I'm too lazy to pipe this in.

        private Config config = new Config();
        private Status status = new Status();

        private bool hasDealtWithLightroomOpen = false;

        private bool timerBeingHandled = false; //Used to handle async events in the timer potentially firing multiple times

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private Bitmap? _trayIconBitmap; // Kept alive so tray icon HICON remains valid


        // Struct representing FLASHWINFO
        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        // Import the FlashWindowEx function from user32.dll
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);



        private void StopFlashing()
        {
            FLASHWINFO flashInfo = new FLASHWINFO
            {
                cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(FLASHWINFO))),
                hwnd = Process.GetCurrentProcess().MainWindowHandle,
                dwFlags = 0, // Stop flashing
                uCount = 0,
                dwTimeout = 0
            };
            FlashWindowEx(ref flashInfo);
        }

        private void Log(string message)
        {
            if (eventsTextBox.InvokeRequired)
            {
                eventsTextBox.Invoke(new Action<string>(Log), message + Environment.NewLine + eventsTextBox.Text);
                return;
            }
            eventsTextBox.Text = message + Environment.NewLine + eventsTextBox.Text;
            LogToFile(message);
        }

        private void LogToFile(string message)
        {
            try
            {
                var folder = GetLogFolder();
                if (string.IsNullOrEmpty(folder)) return;
                Directory.CreateDirectory(folder);
                var file = Path.Combine(folder, $"LightroomSync_{DateTime.Now:yyyy-MM-dd}.log");
                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(file, line + Environment.NewLine);
            }
            catch { /* don't fail app if logging fails */ }
        }

        private string GetLogFolder()
        {
            if (!string.IsNullOrWhiteSpace(config.LogFolder) && Directory.Exists(config.LogFolder))
                return config.LogFolder;
            if (!string.IsNullOrWhiteSpace(config.NetworkFolder) && Directory.Exists(config.NetworkFolder))
                return Path.Combine(config.NetworkFolder, "Logs");
            return "";
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)));
            foreach (var subDir in Directory.GetDirectories(sourceDir))
                CopyDirectory(subDir, Path.Combine(destDir, Path.GetFileName(subDir)));
        }

        private static void AddDirectoryToZip(ZipArchive zipArchive, string sourceDir, string entryPrefix)
        {
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string entryName = Path.Combine(entryPrefix, Path.GetFileName(file));
                zipArchive.CreateEntryFromFile(file, entryName);
            }
            foreach (var subDir in Directory.GetDirectories(sourceDir))
            {
                AddDirectoryToZip(zipArchive, subDir, Path.Combine(entryPrefix, Path.GetFileName(subDir)));
            }
        }

        private async Task UpdateStatusFileOnNetwork()
        {
            await Task.Run(() =>
            {
                try
                {
                    string filePath = config.NetworkFolder + "\\status.txt";
                    File.WriteAllText(filePath, status.ToJson());
                    Log("Updated sync status on network.");
                }
                catch (Exception ex)
                {
                    throw new Exception("ERROR: Failed writing status file: " + ex.Message);
                }
            });
        }

        static async Task ZipFilesAndFolders(string zipFilename, string[] filesToZip, string[] foldersToZip)
        {
            await Task.Run(() =>
            {
                using (ZipArchive zipArchive = ZipFile.Open(zipFilename, ZipArchiveMode.Create))
                {
                    // Zip individual files
                    foreach (string filePath in filesToZip)
                    {
                        if (File.Exists(filePath))
                        {
                            string entryName = Path.GetFileName(filePath);
                            zipArchive.CreateEntryFromFile(filePath, entryName);
                        }
                        else
                        {
                            throw new FileNotFoundException($"File not found: {filePath}");
                        }
                    }

                    // Zip folders
                    foreach (string folderPath in foldersToZip)
                    {
                        if (Directory.Exists(folderPath))
                        {
                            string folderName = Path.GetFileName(folderPath);

                            // Zip files inside the folder
                            string[] files = Directory.GetFiles(folderPath);
                            foreach (string filePath in files)
                            {
                                string entryName = Path.Combine(folderName, Path.GetFileName(filePath));
                                zipArchive.CreateEntryFromFile(filePath, entryName);
                            }
                        }
                        else
                        {
                            throw new DirectoryNotFoundException($"Folder not found: {folderPath}");
                        }
                    }
                }
            });
        }

        private Status? getNetworkStatus()
        {
            string networkStatusFile = config.NetworkFolder + "\\status.txt";
            if (File.Exists(networkStatusFile))
            {

                string jsonContent = File.ReadAllText(networkStatusFile);
                try
                {
                    return JsonConvert.DeserializeObject<Status>(jsonContent);
                }
                catch (JsonException ex)
                {
                    Log("JSON parsing error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Log("Unexpected error: " + ex.Message);
                }
            }
            return null;
        }

        private async Task UploadCatalogs()
        {
            if (Status.LightroomIsOpen())
            {
                Log("Lightroom is open — close it to sync to the network.");
                return;
            }

            //Verify status file says it's safe to work (if it exists, otherwise, we can assume this is the first time)
            Status? loadedStatus = getNetworkStatus();
            if (loadedStatus != null && loadedStatus.isSafeToOverride)
            {
                //Safe to proceed
            }
            else if (loadedStatus != null && loadedStatus.LastUser == Environment.MachineName)
            {
                //Safe to proceed since we're the ones who said it wasn't safe.
            }
            else
            {
                Log("Another machine may be syncing. Wait or check the network status.");
                return;
            }

            status.isSafeToOverride = false;
            try
            {
                await UpdateStatusFileOnNetwork();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return;
            }


            string[] files = Directory.GetFiles(config.LocalFolder, "*.lrcat");

            status.MostRecentVersions = new List<string>();

            foreach (string file in files)
            {
                string catName = Path.GetFileNameWithoutExtension(file);


                string[] filesToZip = { config.LocalFolder + "\\" + catName + ".lrcat" };
                string[] foldersToZip = { config.LocalFolder + "\\" + catName + ".lrcat-data", config.LocalFolder + "\\" + catName + " Helper.lrdata" };

                DateTime lastModified = File.GetLastWriteTime(file);
                string customFormat = catName + " - " + lastModified.ToString("yyyy-MM-dd HH-mm") + ".zip";

                Log($"Preparing {catName} for upload...");
                try
                {
                    await ZipFilesAndFolders(customFormat, filesToZip, foldersToZip);
                    status.MostRecentVersions.Add(customFormat);
                    Log($"Compressed {catName}.");
                }
                catch (Exception ex)
                {
                    Log("ERROR: zipping files and folders: " + ex.Message);
                    return;
                }

                Log($"Uploading {catName} to sync folder...");
                try
                {
                    await Task.Run(() => { File.Move(customFormat, config.NetworkFolder + "\\" + customFormat, true); });
                    Log($"Uploaded {catName}.");
                }
                catch (IOException ex)
                {
                    Log("Error moving: " + ex.Message);
                }
            }

            status.isSafeToOverride = true;
            try
            {
                await UpdateStatusFileOnNetwork();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return;
            }
            config.LastSyncTime = DateTime.Now;
            RefreshLastSyncDisplay();
            Log($"Upload complete — {files.Length} catalog(s) synced.");
        }

        public Form1(bool startMinimized)
        {
            InitializeComponent();

            // Create the NotifyIcon instance
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Lightroom Sync+ - DEV";
            var stream = GetType().Assembly.GetManifestResourceStream("LightroomSync.camera_dev.png");
            if (stream != null)
            {
                _trayIconBitmap = new Bitmap(stream);
                var icon = Icon.FromHandle(_trayIconBitmap.GetHicon());
                trayIcon.Icon = icon;
                this.Icon = icon; // Window title bar too
            }
            else
            {
                trayIcon.Icon = SystemIcons.Application;
            }

            // Create a context menu for the tray icon
            trayMenu = new ContextMenuStrip();
            trayMenu.BackColor = Color.FromArgb(32, 32, 36);
            trayMenu.ForeColor = Color.FromArgb(241, 241, 243);
            trayMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            trayMenu.Items.Add("Restore", null, OnRestore);
            trayMenu.Items.Add("Exit", null, OnExit);

            // Assign the context menu to the tray icon
            trayIcon.ContextMenuStrip = trayMenu;

            // Handle the form's Resize event
            this.Resize += OnResize;
            trayIcon.Click += OnRestore;

            if (startMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                this.ShowInTaskbar = false;
                trayIcon.Visible = true;
            }
        }

        private void OnRestore(object sender, EventArgs e)
        {
            // Restore the form from the system tray
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            trayIcon.Visible = false;
        }

        private void OnExit(object sender, EventArgs e)
        {
            // Clean up resources and close the application
            trayIcon.Dispose();
            System.Windows.Forms.Application.Exit();
        }

        private void OnResize(object sender, EventArgs e)
        {
            // Minimize the form to the system tray when it's minimized
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
                trayIcon.Visible = true;
            }
        }

        private const string ConfigFileName = "config.txt";

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(ConfigFileName))
            {
                string jsonContent = File.ReadAllText(ConfigFileName);
                try
                {
                    Config? loadedConfig = JsonConvert.DeserializeObject<Config>(jsonContent);
                    if (loadedConfig != null)
                    {
                        config = loadedConfig;
                        if (string.IsNullOrEmpty(config.BackupFolder))
                            config.BackupFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Pictures", "LightroomBackups");
                    }
                    else
                    {
                        Log("JSON deserialization failed");
                    }
                }
                catch (JsonException ex) { Log("JSON parsing error: " + ex.Message); }
                catch (Exception ex) { Log("Unexpected error: " + ex.Message); }
            }

            status.LastUser = Environment.MachineName;

            // Pull last sync time from status file if we have no local record or network is newer
            TryUpdateLastSyncFromStatusFile();

            RefreshCatalogDisplay();
            RefreshLastSyncDisplay();

            if (Utils.ShortcutExistsInStartupFolder())
                launchAtStartupToolStripMenuItem.Image = Resources.checkmark;

            if (config.AutoCheckForUpdates)
                autoCheckForUpdatesToolStripMenuItem.Image = Resources.checkmark;

            showActivityLogToolStripMenuItem.Checked = config.ShowActivityLog;
            activityPanel.Visible = config.ShowActivityLog;
            toolTip.SetToolTip(buttonLaunchLightroom, "Start sync first");
            ApplyFormSize();

            CleanupOldLogs();
        }

        private const int FormWidth = 456;
        private const int HeightWithoutActivity = 312;
        private const int HeightWithActivity = 588;

        private void ApplyFormSize()
        {
            var showActivity = config.ShowActivityLog;
            var h = showActivity ? HeightWithActivity : HeightWithoutActivity;
            activityPanel.Visible = showActivity;
            ClientSize = new Size(FormWidth, h);
            MinimumSize = MaximumSize = new Size(FormWidth, h);
        }

        private void CleanupOldLogs()
        {
            try
            {
                var folder = GetLogFolder();
                if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder)) return;
                var cutoff = DateTime.Now.AddDays(-30);
                foreach (var path in Directory.GetFiles(folder, "LightroomSync_*.log"))
                {
                    try
                    {
                        if (File.GetLastWriteTime(path) < cutoff)
                            File.Delete(path);
                    }
                    catch { /* skip if file in use or other error */ }
                }
            }
            catch { /* don't fail app if cleanup fails */ }
        }

        private void showActivityLogToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            config.ShowActivityLog = !config.ShowActivityLog;
            showActivityLogToolStripMenuItem.Checked = config.ShowActivityLog;
            ApplyFormSize();
            File.WriteAllText(ConfigFileName, config.ToJson());
        }

        private void buttonLaunchLightroom_Click(object? sender, EventArgs e)
        {
            // TODO: Launch Lightroom functionality
        }

        private void buttonStartSync_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                lightroomCheckTimer.Enabled = false;
                buttonStartSync.Text = "Start Sync";
                buttonStartSync.Enabled = true;
                buttonStartSync.BackColor = StopSyncAccent;
                buttonStartSync.ForeColor = Color.White;
                toolTip.SetToolTip(buttonStartSync, "");
                buttonLaunchLightroom.Enabled = false;
                toolTip.SetToolTip(buttonLaunchLightroom, "Start sync first");
                SetStatus("Idle");
                SetStatusStrip("", ApertureIconState.DimIdle);
                Log("Stopped watching for changes.");
                return;
            }

            // Validate paths before starting
            if (string.IsNullOrWhiteSpace(config.LocalFolder) || !Directory.Exists(config.LocalFolder))
            {
                MessageBox.Show("Please configure a valid local catalog folder in Settings.", "Settings Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                OpenSettings();
                return;
            }
            if (string.IsNullOrWhiteSpace(config.NetworkFolder) || !Directory.Exists(config.NetworkFolder))
            {
                MessageBox.Show("Please configure a valid sync folder in Settings.", "Settings Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                OpenSettings();
                return;
            }

            bool start = config.SkipStartSyncConfirmation;
            if (!start)
            {
                using var dlg = new StartSyncConfirmDialog();
                start = dlg.ShowDialog(this) == DialogResult.OK;
                if (start && dlg.DontShowAgain)
                    config.SkipStartSyncConfirmation = true;
            }
            if (start)
            {
                timer1.Enabled = true;
                lightroomCheckTimer.Enabled = true;
                buttonStartSync.Text = "Stop Sync";
                buttonLaunchLightroom.Enabled = true;
                toolTip.SetToolTip(buttonLaunchLightroom, "");
                UpdateStopSyncButtonAppearance();
                SetStatus("Syncing");
                SetStatusStrip("Watching for changes...", ApertureIconState.Idle);
                Log("Watching for catalog changes...");
            }
        }

        private void OpenSettings()
        {
            using var dlg = new SettingsDialog(config);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                RefreshCatalogDisplay();
        }

        private void RefreshCatalogDisplay()
        {
            if (InvokeRequired) { Invoke(RefreshCatalogDisplay); return; }
            if (string.IsNullOrWhiteSpace(config.LocalFolder) || !Directory.Exists(config.LocalFolder))
            {
                catalogLabel.Text = "No catalogs configured";
                return;
            }
            var files = Directory.GetFiles(config.LocalFolder, "*.lrcat");
            var names = files.Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();
            catalogLabel.Text = names.Length == 0 ? "No catalogs found" : "Syncing: " + string.Join(", ", names);
        }

        private void TryUpdateLastSyncFromStatusFile()
        {
            if (string.IsNullOrWhiteSpace(config.NetworkFolder) || !Directory.Exists(config.NetworkFolder)) return;
            try
            {
                var statusPath = Path.Combine(config.NetworkFolder, "status.txt");
                if (!File.Exists(statusPath)) return;
                var fileTime = File.GetLastWriteTime(statusPath);
                if (config.LastSyncTime == null || fileTime > config.LastSyncTime.Value)
                {
                    config.LastSyncTime = fileTime;
                }
            }
            catch { /* network may be unavailable */ }
        }

        private void RefreshLastSyncDisplay()
        {
            if (InvokeRequired) { Invoke(RefreshLastSyncDisplay); return; }
            if (config.LastSyncTime == null)
            {
                lastSyncLabel.Text = "Last synced: never";
                return;
            }
            var ago = DateTime.Now - config.LastSyncTime.Value;
            lastSyncLabel.Text = ago.TotalMinutes < 1 ? "Last synced: just now"
                : ago.TotalMinutes < 60 ? $"Last synced: {ago.TotalMinutes:F0} min ago"
                : ago.TotalHours < 24 ? $"Last synced: {ago.TotalHours:F0} hr ago"
                : $"Last synced: {config.LastSyncTime:MMM d, h:mm tt}";
        }

        private void SetStatus(string _)
        {
            // Status shown via statusStripLabel from SetStatusStrip
        }

        private DateTime _statusStripActiveTime;
        private System.Windows.Forms.Timer? _statusStripDelayTimer;

        private void SetStatusStrip(string text, ApertureIconState state)
        {
            if (InvokeRequired) { Invoke(() => SetStatusStrip(text, state)); return; }
            statusStripLabel.Text = string.IsNullOrEmpty(text) ? "Ready" : text;

            if (state == ApertureIconState.Active)
            {
                _statusStripActiveTime = DateTime.Now;
                _statusStripDelayTimer?.Stop();
                spinningSyncIcon.State = ApertureIconState.Active;
            }
            else
            {
                var elapsed = (DateTime.Now - _statusStripActiveTime).TotalSeconds;
                if (spinningSyncIcon.State == ApertureIconState.Active && elapsed < 1.5)
                {
                    var delayMs = (int)((1.5 - elapsed) * 1000);
                    _statusStripDelayTimer?.Stop();
                    _statusStripDelayTimer = new System.Windows.Forms.Timer { Interval = Math.Max(100, delayMs) };
                    _statusStripDelayTimer.Tick += (s, _) =>
                    {
                        _statusStripDelayTimer?.Stop();
                        spinningSyncIcon.State = state;
                    };
                    _statusStripDelayTimer.Start();
                }
                else
                {
                    spinningSyncIcon.State = state;
                    _statusStripDelayTimer?.Stop();
                }
            }
        }

        private void iconGuideToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            var msg = "Icon guide:\n\n" +
                "• Dim (gray): Ready — sync not started\n" +
                "• Normal: Watching for changes\n" +
                "• Spinning: Syncing / uploading catalogs\n" +
                "• Green: Lightroom is open (close it to sync)\n" +
                "• Yellow: Newer catalog available on network\n" +
                "• Red: Error\n" +
                "• White flash: Task completed successfully";
            MessageBox.Show(msg, "Icon Guide", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void openLogFolderToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            var folder = GetLogFolder();
            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("No log folder is configured. Set a Network Folder in Settings, or choose a custom Log Folder.", "Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!Directory.Exists(folder))
            {
                MessageBox.Show($"Log folder does not exist yet:\n{folder}\n\nLogs will be created when you run a sync.", "Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                Process.Start(new ProcessStartInfo { FileName = folder, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open log folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new SettingsDialog(config);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshCatalogDisplay();
            }
        }

        private async void testOutOfSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(config.LocalFolder) || !Directory.Exists(config.LocalFolder))
            {
                MessageBox.Show("Please set a valid Local Folder first.", "Test Out of Sync", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(config.NetworkFolder) || !Directory.Exists(config.NetworkFolder))
            {
                MessageBox.Show("Please set a valid Network Folder first.", "Test Out of Sync", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] lrcatFiles = Directory.GetFiles(config.LocalFolder, "*.lrcat");
            if (lrcatFiles.Length == 0)
            {
                MessageBox.Show("No catalogs found in Local Folder.", "Test Out of Sync", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string firstCatalog = lrcatFiles[0];
            string catName = Path.GetFileNameWithoutExtension(firstCatalog);
            string file1 = Path.Combine(config.LocalFolder, catName + ".lrcat");
            string dir1 = Path.Combine(config.LocalFolder, catName + ".lrcat-data");
            string dir2 = Path.Combine(config.LocalFolder, catName + " Helper.lrdata");

            DateTime futureTime = DateTime.Now.AddHours(1);
            string testZipName = catName + " - " + futureTime.ToString("yyyy-MM-dd HH-mm") + ".zip";

            try
            {
                Log("Creating test scenario...");
                var foldersToZip = new List<string>();
                if (Directory.Exists(dir1)) foldersToZip.Add(dir1);
                if (Directory.Exists(dir2)) foldersToZip.Add(dir2);
                await ZipFilesAndFolders(testZipName, new[] { file1 }, foldersToZip.ToArray());

                string destPath = Path.Combine(config.NetworkFolder, testZipName);
                File.Copy(testZipName, destPath, overwrite: true);
                File.Delete(testZipName);

                var testStatus = new Status
                {
                    LastUser = Environment.MachineName,
                    isSafeToOverride = true,
                    MostRecentVersions = new List<string> { testZipName }
                };
                File.WriteAllText(Path.Combine(config.NetworkFolder, "status.txt"), testStatus.ToJson());

                Log("Test scenario ready. Checking for updates...");
                HandleTimerEvent();
            }
            catch (Exception ex)
            {
                Log("ERROR creating test scenario: " + ex.Message);
                MessageBox.Show("Failed to create test scenario: " + ex.Message, "Test Out of Sync", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(ConfigFileName, config.ToJson());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timerBeingHandled == true) { }
            HandleTimerEvent();
        }

        private static readonly Color StopSyncAccent = Color.FromArgb(0, 122, 204);
        private static readonly Color StopSyncDisabledBg = Color.FromArgb(55, 55, 62);
        private static readonly Color StopSyncDisabledFg = Color.FromArgb(100, 100, 110);

        private void UpdateStopSyncButtonAppearance()
        {
            bool lrOpen = Status.LightroomIsOpen();
            buttonStartSync.Enabled = !lrOpen;
            if (lrOpen && timer1.Enabled)
                spinningSyncIcon.State = ApertureIconState.LightroomOpen;
            if (lrOpen)
            {
                buttonStartSync.BackColor = StopSyncDisabledBg;
                buttonStartSync.ForeColor = StopSyncDisabledFg;
                toolTip.SetToolTip(buttonStartSync, "Close Lightroom to stop sync");
            }
            else
            {
                buttonStartSync.BackColor = StopSyncAccent;
                buttonStartSync.ForeColor = Color.White;
                toolTip.SetToolTip(buttonStartSync, "");
            }
        }

        private void lightroomCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!timer1.Enabled) return;
            if (InvokeRequired) { Invoke(lightroomCheckTimer_Tick, sender, e); return; }
            UpdateStopSyncButtonAppearance();
        }

        private async void HandleTimerEvent()
        {
            timerBeingHandled = true;

            if (Status.LightroomIsOpen() && hasDealtWithLightroomOpen == false)
            {
                SetStatus("Waiting for Lightroom to close...");
                SetStatusStrip("Waiting for Lightroom to close...", ApertureIconState.LightroomOpen);
                timer1.Enabled = false;

                Status? loadedStatus = getNetworkStatus();
                if (loadedStatus != null && loadedStatus.isSafeToOverride == false && loadedStatus.LastUser != Environment.MachineName)
                {
                    Alert alert = new(loadedStatus.LastUser);
                    DialogResult result = alert.ShowDialog();
                    if (result == DialogResult.Yes)
                    {
                        Process[] processes = Process.GetProcessesByName("Lightroom");
                        if (processes.Length > 0)
                        {
                            processes[0].Kill();
                            Log("Lightroom process killed. Please close Lightroom on " + loadedStatus.LastUser + " before trying again.");
                            StopFlashing();
                            SetStatus("Syncing");
                            SetStatusStrip("Watching for changes...", ApertureIconState.Idle);
                            timer1.Enabled = true;
                            timerBeingHandled = false;
                            return;
                        }
                        else
                        {
                            Log("ERROR: Couldn't find Lightroom process to kill! Either this is a bug or you closed it manually. Please restart this application (and consider filing a bug report on GitHub).");
                            SetStatus("Syncing");
                            SetStatusStrip("Error: Couldn't find Lightroom process", ApertureIconState.Error);
                            return;
                        }
                    }

                    // Continue if user selected DialogResult.No, since that means they want to wipe the existing status
                Log("Taking over sync status from other machine.");
                    StopFlashing();
                }

                Log("Lightroom is open — notifying other machines to wait.");
                hasDealtWithLightroomOpen = true;
                status.isSafeToOverride = false;
                status.LastUser = Environment.MachineName;
                await UpdateStatusFileOnNetwork();
                SetStatus("Syncing");
                SetStatusStrip("Waiting for Lightroom to close...", ApertureIconState.LightroomOpen);
                timer1.Enabled = true;
                timerBeingHandled = false;
            }
            else if (Status.LightroomIsOpen() == false && hasDealtWithLightroomOpen == true)
            {
                SetStatus("Uploading catalogs...");
                SetStatusStrip("Uploading catalogs...", ApertureIconState.Active);
                timer1.Enabled = false;
                await UploadCatalogs();
                hasDealtWithLightroomOpen = false;
                SetStatus("Syncing");
                SetStatusStrip("Successfully uploaded", ApertureIconState.Idle);
                timer1.Enabled = true;
                timerBeingHandled = false;
            }
            else if (Status.LightroomIsOpen() == false && hasDealtWithLightroomOpen == false)
            {
                SetStatus("Syncing");
                if (statusStripLabel.Text != "Successfully uploaded")
                    SetStatusStrip("Checking for updates...", ApertureIconState.Idle);
                timer1.Enabled = false;
                Status? loadedStatus = getNetworkStatus();
                if (loadedStatus == null)
                {
                    SetStatus("Syncing");
                    if (statusStripLabel.Text != "Successfully uploaded")
                        SetStatusStrip("Watching for changes...", ApertureIconState.Idle);
                    timer1.Enabled = true;
                    timerBeingHandled = false;
                    return;
                }

                if (string.IsNullOrWhiteSpace(config.LocalFolder) || !Directory.Exists(config.LocalFolder))
                {
                    SetStatus("Syncing");
                    if (statusStripLabel.Text != "Successfully uploaded")
                        SetStatusStrip("Watching for changes...", ApertureIconState.Idle);
                    timer1.Enabled = true;
                    timerBeingHandled = false;
                    return;
                }

                string[] files = Directory.GetFiles(config.LocalFolder, "*.lrcat");
                string[] timestamped = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    string catName = Path.GetFileNameWithoutExtension(files[i]);
                    DateTime lastModified = File.GetLastWriteTime(files[i]);
                    timestamped[i] = catName + " - " + lastModified.ToString("yyyy-MM-dd HH-mm") + ".zip";
                }

                foreach (string catalog in loadedStatus.MostRecentVersions)
                {
                    if (!timestamped.Contains(catalog))
                    {
                        //We do not have the most recent version.
                        string catName = catalog.Substring(0, catalog.Length - 23); //23 is len(" - yyyy-MM-dd HH-mm.zip")
                        string file1 = Path.Combine(config.LocalFolder, catName + ".lrcat");
                        string dir1 = Path.Combine(config.LocalFolder, catName + ".lrcat-data");
                        string dir2 = Path.Combine(config.LocalFolder, catName + " Helper.lrdata");

                        SetStatusStrip("Newer catalog available", ApertureIconState.Warning);

                        // Only ask Backup/Discard if we have an existing catalog to replace
                        var choice = BackupOrDiscardChoice.Discard;
                        if (File.Exists(file1) || Directory.Exists(dir1) || Directory.Exists(dir2))
                        {
                            var backupDialog = new BackupOrDiscardDialog(catName);
                            backupDialog.ShowDialog(this);
                            choice = backupDialog.Choice;
                        }

                        if (choice == BackupOrDiscardChoice.Cancel)
                        {
                            Log($"Update skipped for {catName}.");
                            continue;
                        }

                        if (choice == BackupOrDiscardChoice.Backup && !string.IsNullOrWhiteSpace(config.BackupFolder))
                        {
                            try
                            {
                                Directory.CreateDirectory(config.BackupFolder);
                                string backupZipPath = Path.Combine(config.BackupFolder, catName + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".zip");
                                await Task.Run(() =>
                                {
                                    using (var zipArchive = ZipFile.Open(backupZipPath, ZipArchiveMode.Create))
                                    {
                                        if (File.Exists(file1))
                                            zipArchive.CreateEntryFromFile(file1, catName + ".lrcat");
                                        if (Directory.Exists(dir1))
                                            AddDirectoryToZip(zipArchive, dir1, catName + ".lrcat-data");
                                        if (Directory.Exists(dir2))
                                            AddDirectoryToZip(zipArchive, dir2, catName + " Helper.lrdata");
                                    }
                                });
                                Log($"Backed up {catName} before replacing.");
                            }
                            catch (Exception ex)
                            {
                                Log("ERROR backing up catalog: " + ex.Message + " - Aborting update.");
                                SetStatus("Syncing");
                                SetStatusStrip("Backup failed: " + ex.Message, ApertureIconState.Error);
                                timer1.Enabled = true;
                                timerBeingHandled = false;
                                return;
                            }
                        }
                        else if (choice == BackupOrDiscardChoice.Backup && string.IsNullOrWhiteSpace(config.BackupFolder))
                        {
                            Log("Backup folder not set. Please set Backup Folder and try again. Aborting update.");
                            SetStatus("Syncing");
                            if (statusStripLabel.Text != "Successfully uploaded")
                                SetStatusStrip("Watching for changes...", ApertureIconState.Idle);
                            timer1.Enabled = true;
                            timerBeingHandled = false;
                            return;
                        }

                        SetStatus("Downloading " + catName + "...");
                        SetStatusStrip($"Downloading {catName}...", ApertureIconState.Active);
                        Log($"Newer version of {catName} found. Downloading...");
                        try
                        {
                            await Task.Run(() => { File.Copy(Path.Combine(config.NetworkFolder, catalog), catalog); });
                            Log($"Downloaded. Replacing local {catName}...");

                            try
                            {
                                if (File.Exists(file1)) File.Delete(file1);
                                if (Directory.Exists(dir1)) Directory.Delete(dir1, recursive: true);
                                if (Directory.Exists(dir2)) Directory.Delete(dir2, recursive: true);
                            }
                            catch (IOException ex)
                            {
                                Log("An I/O error occurred: " + ex.Message);
                                Log("YOU SHOULD MANUALLY EXTRACT THE ZIP TO RECOVER YOUR CATALOG");
                                return;
                            }
                            catch (UnauthorizedAccessException ex)
                            {
                                Log("Unauthorized access error occurred: " + ex.Message);
                                Log("YOU SHOULD MANUALLY EXTRACT THE ZIP TO RECOVER YOUR CATALOG");
                                return;
                            }
                            catch (Exception ex)
                            {
                                Log("An error occurred: " + ex.Message);
                                Log("YOU SHOULD MANUALLY EXTRACT THE ZIP TO RECOVER YOUR CATALOG");
                                return;
                            }

                            Log("Extracting...");
                            try
                            {
                                string zipPath = catalog;
                                await Task.Run(() =>
                                {
                                    ZipFile.ExtractToDirectory(zipPath, config.LocalFolder);
                                });
                                // Set extracted .lrcat's LastWriteTime to match the zip's timestamp,
                                // so the next sync check sees us as in-sync (avoids infinite loop)
                                string timestampStr = catalog.Substring(catalog.Length - 20, 16); // "yyyy-MM-dd HH-mm"
                                if (DateTime.TryParseExact(timestampStr, "yyyy-MM-dd HH-mm", null, System.Globalization.DateTimeStyles.None, out DateTime zipTimestamp)
                                    && File.Exists(file1))
                                {
                                    File.SetLastWriteTime(file1, zipTimestamp);
                                }
                                Log("Extraction complete.");
                            }
                            catch (Exception ex)
                            {
                                Log("An error occurred while unzipping the file: " + ex.Message);
                                Log("YOU SHOULD MANUALLY EXTRACT THE ZIP TO RECOVER YOUR CATALOG");
                            }

                            //You know what? If this file I just created has errors in deleting, I want someone to go file a bug report.
                            //That's absurd, and I'm not adding another wall of error handling around it.
                            File.Delete(catalog);
                            config.LastSyncTime = DateTime.Now;
                            RefreshLastSyncDisplay();
                            Log($"Downloaded and applied {catName}.");
                        }
                        catch (IOException ex)
                        {
                            Log("Error copying: " + ex.Message);
                        }
                    }
                }

                SetStatus("Syncing");
                if (statusStripLabel.Text != "Successfully uploaded")
                    SetStatusStrip("Watching for changes...", ApertureIconState.Idle);
                timer1.Enabled = true;
                timerBeingHandled = false;
            }
        }

        private async void CheckForUpdates(bool silently)
        {
            string version = "ERR NOT SET";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    version = await client.GetStringAsync("https://github.com/alex-pattison/LightroomSyncPlus/raw/master/latestVersion.txt");
                }
                catch (Exception ex)
                {
                    Log($"Error checking for new version: {ex.Message}");
                    if (silently)
                    {
                        return;
                    }
                    var result = MessageBox.Show("Sorry, I couldn't find the version number. Do you want to go to the website to check?", "Error!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Yes)
                    {
                        Utils.OpenURL("https://github.com/alex-pattison/LightroomSyncPlus/releases");
                    }
                    return;
                }
            }

            var parsed = Version.Parse(version);

            if (parsed.CompareTo(Version.Parse(currentVersion)) != 0)
            {
                var dialog = "There is a new version! Want to go get it?" + Environment.NewLine + Environment.NewLine + "New Version: " + parsed.ToString() + Environment.NewLine + "Your Version: " + currentVersion.ToString();
                var result = MessageBox.Show(dialog, "New Version!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    Utils.OpenURL("https://github.com/alex-pattison/LightroomSyncPlus/releases");
                }
            }
            else if (!silently)
            {
                MessageBox.Show("You expected an update, but it was me! Dio!", "Up To Date!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void submitABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.OpenURL("https://github.com/alex-pattison/LightroomSyncPlus/issues");
        }

        private void gitHubPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.OpenURL("https://github.com/alex-pattison/LightroomSyncPlus");
        }

        private void minimizeToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void launchAtStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Either remove or add from startup as a toggle.
            if (Utils.ShortcutExistsInStartupFolder())
            {
                Utils.DeleteShortcutFromStartupFolder();
                launchAtStartupToolStripMenuItem.Image = null;
            }
            else
            {
                string assemblyLocation = Assembly.GetEntryAssembly().Location;
                string executablePath = Path.GetDirectoryName(assemblyLocation);
                string appPath = Path.Combine(executablePath, "LightroomSyncPlus.exe");
                launchAtStartupToolStripMenuItem.Image = Resources.checkmark;

                Utils.CreateShortcutInStartupFolder(appPath);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lightroom Sync+" + Environment.NewLine + "Copyright 2023 Anthony Bryan" + Environment.NewLine + Environment.NewLine + "Version " + currentVersion);
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates(false);
        }

        private void autoCheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config.AutoCheckForUpdates = !config.AutoCheckForUpdates;
            if (config.AutoCheckForUpdates)
            {
                autoCheckForUpdatesToolStripMenuItem.Image = Resources.checkmark;
            } 
            else
            {
                autoCheckForUpdatesToolStripMenuItem.Image = null;
            }
        }
    }
}