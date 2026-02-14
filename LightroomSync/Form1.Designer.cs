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
            var spacing = 28;

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

            // --- Unified content panel: icon + status + catalog + last sync + buttons ---
            var contentPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 140,
                Padding = new Padding(spacing),
                BackColor = panelBg
            };

            spinningSyncIcon = new SpinningSyncIcon();
            spinningSyncIcon.Location = new Point(0, 28);
            spinningSyncIcon.Size = new Size(72, 72);

            var textFlowPanel = new FlowLayoutPanel
            {
                Location = new Point(108, 28),
                Size = new Size(320, 90),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = panelBg,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };

            statusStripLabel = new Label();
            statusStripLabel.Text = "Ready";
            statusStripLabel.ForeColor = textPrimary;
            statusStripLabel.Font = new Font("Segoe UI Semibold", 11F);
            statusStripLabel.AutoSize = true;
            statusStripLabel.MaximumSize = new Size(320, 0);
            statusStripLabel.Margin = new Padding(0, 0, 0, 4);

            catalogLabel = new Label();
            catalogLabel.Text = "No catalogs configured";
            catalogLabel.ForeColor = textMuted;
            catalogLabel.Font = new Font("Segoe UI", 9F);
            catalogLabel.AutoSize = true;
            catalogLabel.MaximumSize = new Size(320, 0);
            catalogLabel.Margin = new Padding(0, 0, 0, 4);

            lastSyncLabel = new Label();
            lastSyncLabel.Text = "Last synced: never";
            lastSyncLabel.ForeColor = textMuted;
            lastSyncLabel.Font = new Font("Segoe UI", 9F);
            lastSyncLabel.AutoSize = true;
            lastSyncLabel.MaximumSize = new Size(320, 0);
            lastSyncLabel.Margin = new Padding(0, 0, 0, 0);

            textFlowPanel.Controls.Add(statusStripLabel);
            textFlowPanel.Controls.Add(catalogLabel);
            textFlowPanel.Controls.Add(lastSyncLabel);

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

            buttonLaunchLightroom = new Button();
            buttonLaunchLightroom.Text = "Launch Lightroom";
            buttonLaunchLightroom.FlatStyle = FlatStyle.Flat;
            buttonLaunchLightroom.BackColor = inputBg;
            buttonLaunchLightroom.ForeColor = textMuted;
            buttonLaunchLightroom.Font = new Font("Segoe UI Semibold", 10F);
            buttonLaunchLightroom.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 78);
            buttonLaunchLightroom.FlatAppearance.BorderSize = 1;
            buttonLaunchLightroom.Size = new Size(120, 36);
            buttonLaunchLightroom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonLaunchLightroom.Enabled = false;
            buttonLaunchLightroom.Click += buttonLaunchLightroom_Click;

            contentPanel.Controls.Add(spinningSyncIcon);
            contentPanel.Controls.Add(textFlowPanel);
            contentPanel.Controls.Add(buttonStartSync);
            contentPanel.Controls.Add(buttonLaunchLightroom);

            contentPanel.Resize += (s, e) =>
            {
                var p = (Panel)s!;
                var right = p.ClientSize.Width - spacing;
                var btnTop = (p.ClientSize.Height - 36) / 2;
                buttonStartSync.Left = right - 120 - 120 - 10;
                buttonStartSync.Top = btnTop;
                buttonLaunchLightroom.Left = right - 120;
                buttonLaunchLightroom.Top = btnTop;
            };

            // --- Activity log ---
            activityPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0), BackColor = bgDark };

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
            showActivityLogToolStripMenuItem = new ToolStripMenuItem();
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                settingsToolStripMenuItem,
                showActivityLogToolStripMenuItem,
                launchAtStartupToolStripMenuItem,
                autoCheckForUpdatesToolStripMenuItem,
                testOutOfSyncToolStripMenuItem,
                minimizeToTrayToolStripMenuItem,
                exitToolStripMenuItem
            });

            showActivityLogToolStripMenuItem.Text = "Show activity log";
            showActivityLogToolStripMenuItem.Click += showActivityLogToolStripMenuItem_Click;

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
            iconGuideToolStripMenuItem = new ToolStripMenuItem();
            openLogFolderToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                iconGuideToolStripMenuItem,
                openLogFolderToolStripMenuItem,
                new ToolStripSeparator(),
                submitABugToolStripMenuItem,
                gitHubPageToolStripMenuItem,
                checkForUpdatesToolStripMenuItem,
                aboutToolStripMenuItem
            });

            iconGuideToolStripMenuItem.Text = "Icon guide...";
            iconGuideToolStripMenuItem.Click += iconGuideToolStripMenuItem_Click;

            openLogFolderToolStripMenuItem.Text = "Open log folder";
            openLogFolderToolStripMenuItem.Click += openLogFolderToolStripMenuItem_Click;

            submitABugToolStripMenuItem.Text = "Submit a bug";
            submitABugToolStripMenuItem.Click += submitABugToolStripMenuItem_Click;

            gitHubPageToolStripMenuItem.Text = "GitHub";
            gitHubPageToolStripMenuItem.Click += gitHubPageToolStripMenuItem_Click;

            checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            checkForUpdatesToolStripMenuItem.Click += checkForUpdatesToolStripMenuItem_Click;

            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;

            activityHeader.Controls.Add(labelActivity);

            // --- Layout ---
            activityPanel.Controls.Add(eventsTextBox);
            activityPanel.Controls.Add(activityHeader);

            mainPanel.Controls.Add(activityPanel);
            mainPanel.Controls.Add(contentPanel);

            // --- Form ---
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = bgDark;
            ForeColor = textPrimary;
            ClientSize = new Size(520, 420);
            Controls.Add(mainPanel);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
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

        private Button buttonLaunchLightroom;
        private Label catalogLabel;
        private Label lastSyncLabel;
        private Button buttonStartSync;
        private Label statusStripLabel;
        private SpinningSyncIcon spinningSyncIcon;
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
        private ToolStripMenuItem iconGuideToolStripMenuItem;
        private ToolStripMenuItem openLogFolderToolStripMenuItem;
        private ToolStripMenuItem showActivityLogToolStripMenuItem;
        private Panel activityPanel;
    }
}
