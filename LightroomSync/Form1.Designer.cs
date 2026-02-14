namespace LightroomSync
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _trayIconBitmap?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            var bgDark = Color.FromArgb(24, 24, 26);
            var panelBg = Color.FromArgb(32, 32, 36);
            var inputBg = Color.FromArgb(43, 43, 48);
            var textPrimary = Color.FromArgb(241, 241, 243);
            var textMuted = Color.FromArgb(161, 161, 170);
            var accent = Color.FromArgb(0, 122, 204);
            var spacing = 20;

            var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(spacing), BackColor = bgDark };

            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            launchAtStartupToolStripMenuItem = new ToolStripMenuItem();
            autoCheckForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            testOutOfSyncToolStripMenuItem = new ToolStripMenuItem();
            minimizeToTrayToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            submitABugToolStripMenuItem = new ToolStripMenuItem();
            gitHubPageToolStripMenuItem = new ToolStripMenuItem();
            checkForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();

            // --- Dashboard: Status + Actions ---
            var dashboardPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                Padding = new Padding(spacing),
                BackColor = panelBg
            };

            statusLabel = new Label();
            statusLabel.Text = "Idle";
            statusLabel.ForeColor = textMuted;
            statusLabel.Font = new Font("Segoe UI", 11F);
            statusLabel.Location = new Point(spacing, 12);
            statusLabel.AutoSize = true;

            catalogLabel = new Label();
            catalogLabel.Text = "No catalogs configured";
            catalogLabel.ForeColor = textMuted;
            catalogLabel.Font = new Font("Segoe UI", 9F);
            catalogLabel.Location = new Point(spacing, 36);
            catalogLabel.AutoSize = true;
            catalogLabel.MaximumSize = new Size(400, 0);

            lastSyncLabel = new Label();
            lastSyncLabel.Text = "Last synced: never";
            lastSyncLabel.ForeColor = textMuted;
            lastSyncLabel.Font = new Font("Segoe UI", 9F);
            lastSyncLabel.Location = new Point(spacing, 54);
            lastSyncLabel.AutoSize = true;

            buttonStartSync = new Button();
            buttonStartSync.Text = "Start Sync";
            buttonStartSync.FlatStyle = FlatStyle.Flat;
            buttonStartSync.BackColor = accent;
            buttonStartSync.ForeColor = Color.White;
            buttonStartSync.Font = new Font("Segoe UI Semibold", 10F);
            buttonStartSync.FlatAppearance.BorderSize = 0;
            buttonStartSync.Size = new Size(120, 36);
            buttonStartSync.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonStartSync.Click += buttonStartSync_Click;

            // --- Activity log ---
            var activityPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0), BackColor = bgDark };

            var activityHeader = new Panel { Dock = DockStyle.Top, Height = 28, BackColor = bgDark };

            labelActivity = new Label();
            labelActivity.Text = "Activity";
            labelActivity.ForeColor = textMuted;
            labelActivity.Font = new Font("Segoe UI", 9F);
            labelActivity.Location = new Point(0, 4);
            labelActivity.AutoSize = true;

            eventsTextBox = new TextBox();
            eventsTextBox.BackColor = inputBg;
            eventsTextBox.ForeColor = textPrimary;
            eventsTextBox.BorderStyle = BorderStyle.FixedSingle;
            eventsTextBox.Font = new Font("Consolas", 9F);
            eventsTextBox.Multiline = true;
            eventsTextBox.ScrollBars = ScrollBars.Vertical;
            eventsTextBox.Dock = DockStyle.Fill;

            timer1 = new System.Windows.Forms.Timer(components) { Interval = 5000, Enabled = false };
            timer1.Tick += timer1_Tick;

            lightroomCheckTimer = new System.Windows.Forms.Timer(components) { Interval = 500, Enabled = false };
            lightroomCheckTimer.Tick += lightroomCheckTimer_Tick;

            toolTip = new ToolTip();

            // --- Menu ---
            menuStrip1.BackColor = panelBg;
            menuStrip1.ForeColor = textPrimary;
            menuStrip1.Font = new Font("Segoe UI", 9F);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });

            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                settingsToolStripMenuItem,
                launchAtStartupToolStripMenuItem,
                autoCheckForUpdatesToolStripMenuItem,
                testOutOfSyncToolStripMenuItem,
                minimizeToTrayToolStripMenuItem,
                exitToolStripMenuItem
            });

            settingsToolStripMenuItem.Text = "Settings...";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;

            launchAtStartupToolStripMenuItem.Text = "Launch at startup";
            launchAtStartupToolStripMenuItem.Click += launchAtStartupToolStripMenuItem_Click;

            autoCheckForUpdatesToolStripMenuItem.Text = "Auto-check for updates";
            autoCheckForUpdatesToolStripMenuItem.Click += autoCheckForUpdatesToolStripMenuItem_Click;

            testOutOfSyncToolStripMenuItem.Text = "Test out of sync...";
            testOutOfSyncToolStripMenuItem.Click += testOutOfSyncToolStripMenuItem_Click;

            minimizeToTrayToolStripMenuItem.Text = "Minimize to tray";
            minimizeToTrayToolStripMenuItem.Click += minimizeToTrayToolStripMenuItem_Click;

            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;

            helpToolStripMenuItem.Text = "Help";
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                submitABugToolStripMenuItem,
                gitHubPageToolStripMenuItem,
                checkForUpdatesToolStripMenuItem,
                aboutToolStripMenuItem
            });

            submitABugToolStripMenuItem.Text = "Submit a bug";
            submitABugToolStripMenuItem.Click += submitABugToolStripMenuItem_Click;

            gitHubPageToolStripMenuItem.Text = "GitHub";
            gitHubPageToolStripMenuItem.Click += gitHubPageToolStripMenuItem_Click;

            checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            checkForUpdatesToolStripMenuItem.Click += checkForUpdatesToolStripMenuItem_Click;

            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;

            activityHeader.Controls.Add(labelActivity);

            // --- Layout --- (Fill first, then Top)
            activityPanel.Controls.Add(eventsTextBox);
            activityPanel.Controls.Add(activityHeader);

            buttonStartSync.Location = new Point(380, 28);

            dashboardPanel.Controls.Add(statusLabel);
            dashboardPanel.Controls.Add(catalogLabel);
            dashboardPanel.Controls.Add(lastSyncLabel);
            dashboardPanel.Controls.Add(buttonStartSync);

            mainPanel.Controls.Add(activityPanel);
            mainPanel.Controls.Add(dashboardPanel);

            dashboardPanel.Resize += (s, e) =>
            {
                var w = dashboardPanel.ClientSize.Width;
                buttonStartSync.Left = w - spacing - 120;
            };

            // --- Form ---
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = bgDark;
            ForeColor = textPrimary;
            ClientSize = new Size(520, 420);
            Controls.Add(mainPanel);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(420, 320);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lightroom Sync+";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;

            SuspendLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label statusLabel;
        private Label catalogLabel;
        private Label lastSyncLabel;
        private Button buttonStartSync;
        private Label labelActivity;
        private TextBox eventsTextBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer lightroomCheckTimer;
        private ToolTip toolTip;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem launchAtStartupToolStripMenuItem;
        private ToolStripMenuItem testOutOfSyncToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem minimizeToTrayToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem submitABugToolStripMenuItem;
        private ToolStripMenuItem gitHubPageToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private ToolStripMenuItem autoCheckForUpdatesToolStripMenuItem;
    }
}
