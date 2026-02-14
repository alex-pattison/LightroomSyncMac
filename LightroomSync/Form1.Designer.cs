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
            testOutOfSyncToolStripMenuItem = new ToolStripMenuItem();
            minimizeToTrayToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            submitABugToolStripMenuItem = new ToolStripMenuItem();
            gitHubPageToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();

            // --- Content panel (icon + text aligned, buttons below) ---
            var contentPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 196,
                Padding = new Padding(spacing),
                BackColor = panelBg
            };

            // Icon + text row: use table for proper vertical alignment
            var iconTextRow = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 88,
                BackColor = panelBg,
                Padding = Padding.Empty,
                Margin = Padding.Empty,
                ColumnCount = 2,
                RowCount = 1
            };
            iconTextRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 72));
            iconTextRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            iconTextRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            spinningSyncIcon = new SpinningSyncIcon();
            spinningSyncIcon.Location = new Point(0, 8);  // 8 = (88-72)/2 for vertical center
            spinningSyncIcon.Size = new Size(72, 72);

            var iconCell = new Panel { Dock = DockStyle.Fill, BackColor = panelBg };
            iconCell.Controls.Add(spinningSyncIcon);

            var textContainer = new Panel { Dock = DockStyle.Fill, BackColor = panelBg, Padding = new Padding(16, 0, 16, 0) };
            var textFlowPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = panelBg,
                Padding = new Padding(0, 14, 0, 14),
                Margin = Padding.Empty
            };

            statusStripLabel = new Label();
            statusStripLabel.Text = "Ready";
            statusStripLabel.ForeColor = textPrimary;
            statusStripLabel.Font = new Font("Segoe UI Semibold", 11F);
            statusStripLabel.AutoSize = true;
            statusStripLabel.MaximumSize = new Size(400, 0);
            statusStripLabel.Margin = new Padding(0, 0, 0, 4);

            catalogLabel = new Label();
            catalogLabel.Text = "No catalogs configured";
            catalogLabel.ForeColor = textMuted;
            catalogLabel.Font = new Font("Segoe UI", 9F);
            catalogLabel.AutoSize = true;
            catalogLabel.MaximumSize = new Size(400, 0);
            catalogLabel.Margin = new Padding(0, 0, 0, 4);

            lastSyncLabel = new Label();
            lastSyncLabel.Text = "Last synced: never";
            lastSyncLabel.ForeColor = textMuted;
            lastSyncLabel.Font = new Font("Segoe UI", 9F);
            lastSyncLabel.AutoSize = true;
            lastSyncLabel.MaximumSize = new Size(400, 0);
            lastSyncLabel.Margin = new Padding(0, 0, 0, 0);

            textFlowPanel.Controls.Add(statusStripLabel);
            textFlowPanel.Controls.Add(catalogLabel);
            textFlowPanel.Controls.Add(lastSyncLabel);
            textContainer.Controls.Add(textFlowPanel);

            iconTextRow.Controls.Add(iconCell, 0, 0);
            iconTextRow.Controls.Add(textContainer, 1, 0);

            // Buttons row - equal size columns for identical button dimensions
            var buttonRow = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 52,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = panelBg,
                Padding = new Padding(0, 10, 0, 10)
            };
            buttonRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttonRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttonRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

            buttonStartSync = new Button();
            buttonStartSync.Text = "Start Sync";
            buttonStartSync.FlatStyle = FlatStyle.Flat;
            buttonStartSync.BackColor = accent;
            buttonStartSync.ForeColor = Color.White;
            buttonStartSync.Font = new Font("Segoe UI Semibold", 10F);
            buttonStartSync.FlatAppearance.BorderSize = 0;
            buttonStartSync.Size = new Size(110, 36);
            buttonStartSync.Dock = DockStyle.Fill;
            buttonStartSync.Margin = new Padding(6, 0, 6, 0);
            buttonStartSync.Click += buttonStartSync_Click;

            buttonLaunchLightroom = new Button();
            buttonLaunchLightroom.Text = "Launch LR";
            buttonLaunchLightroom.FlatStyle = FlatStyle.Flat;
            buttonLaunchLightroom.BackColor = inputBg;
            buttonLaunchLightroom.ForeColor = textMuted;
            buttonLaunchLightroom.Font = new Font("Segoe UI Semibold", 10F);
            buttonLaunchLightroom.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 78);
            buttonLaunchLightroom.FlatAppearance.BorderSize = 1;
            buttonLaunchLightroom.Size = new Size(110, 36);
            buttonLaunchLightroom.Dock = DockStyle.Fill;
            buttonLaunchLightroom.Margin = new Padding(6, 0, 6, 0);
            buttonLaunchLightroom.Enabled = false;
            buttonLaunchLightroom.Click += buttonLaunchLightroom_Click;

            buttonRow.Controls.Add(buttonStartSync, 0, 0);
            buttonRow.Controls.Add(buttonLaunchLightroom, 1, 0);

            contentPanel.Controls.Add(buttonRow);
            contentPanel.Controls.Add(iconTextRow);

            // --- Activity log (below content) ---
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
            ClientSize = new Size(456, 280);
            Controls.Add(mainPanel);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            // Icon set in Form1 constructor from aperture (red for dev)
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lightroom Sync+ DEV";
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
        private ToolStripMenuItem iconGuideToolStripMenuItem;
        private ToolStripMenuItem openLogFolderToolStripMenuItem;
        private ToolStripMenuItem showActivityLogToolStripMenuItem;
        private Panel activityPanel;
    }
}
