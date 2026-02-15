namespace LightroomSync
{
    partial class SettingsDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var bgDark = Color.FromArgb(24, 24, 26);
            var panelBg = Color.FromArgb(32, 32, 36);
            var inputBg = Color.FromArgb(43, 43, 48);
            var inputBorder = Color.FromArgb(60, 60, 66);
            var textPrimary = Color.FromArgb(241, 241, 243);
            var textMuted = Color.FromArgb(161, 161, 170);
            var accent = Color.FromArgb(0, 122, 204);
            var spacing = 20;

            lblLocalFolder = new Label();
            txtLocalFolder = new TextBox();
            btnBrowseLocal = new Button();
            lblSyncFolder = new Label();
            txtSyncFolder = new TextBox();
            btnBrowseSync = new Button();
            chkUseDefaultBackup = new CheckBox();
            lblBackupFolder = new Label();
            txtBackupFolder = new TextBox();
            btnBrowseBackup = new Button();
            btnSave = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // Local folder
            lblLocalFolder.Text = "Local catalog folder";
            lblLocalFolder.ForeColor = textMuted;
            lblLocalFolder.Font = new Font("Segoe UI", 9F);
            lblLocalFolder.Location = new Point(spacing, spacing);
            lblLocalFolder.AutoSize = true;

            txtLocalFolder.BackColor = inputBg;
            txtLocalFolder.ForeColor = textPrimary;
            txtLocalFolder.BorderStyle = BorderStyle.FixedSingle;
            txtLocalFolder.Font = new Font("Consolas", 9.5F);
            txtLocalFolder.Location = new Point(spacing, 42);
            txtLocalFolder.Size = new Size(420, 28);
            txtLocalFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtLocalFolder.TextChanged += txtLocalFolder_TextChanged;

            btnBrowseLocal.Text = "Browse";
            btnBrowseLocal.FlatStyle = FlatStyle.Flat;
            btnBrowseLocal.BackColor = inputBg;
            btnBrowseLocal.ForeColor = textPrimary;
            btnBrowseLocal.Font = new Font("Segoe UI", 9F);
            btnBrowseLocal.FlatAppearance.BorderColor = inputBorder;
            btnBrowseLocal.Location = new Point(448, 40);
            btnBrowseLocal.Size = new Size(80, 30);
            btnBrowseLocal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseLocal.Click += btnBrowseLocal_Click;

            // Sync folder
            lblSyncFolder.Text = "Sync folder (Google Drive, Dropbox, etc.)";
            lblSyncFolder.ForeColor = textMuted;
            lblSyncFolder.Font = new Font("Segoe UI", 9F);
            lblSyncFolder.Location = new Point(spacing, 88);
            lblSyncFolder.AutoSize = true;

            txtSyncFolder.BackColor = inputBg;
            txtSyncFolder.ForeColor = textPrimary;
            txtSyncFolder.BorderStyle = BorderStyle.FixedSingle;
            txtSyncFolder.Font = new Font("Consolas", 9.5F);
            txtSyncFolder.Location = new Point(spacing, 110);
            txtSyncFolder.Size = new Size(420, 28);
            txtSyncFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSyncFolder.TextChanged += txtSyncFolder_TextChanged;

            btnBrowseSync.Text = "Browse";
            btnBrowseSync.FlatStyle = FlatStyle.Flat;
            btnBrowseSync.BackColor = inputBg;
            btnBrowseSync.ForeColor = textPrimary;
            btnBrowseSync.Font = new Font("Segoe UI", 9F);
            btnBrowseSync.FlatAppearance.BorderColor = inputBorder;
            btnBrowseSync.Location = new Point(448, 108);
            btnBrowseSync.Size = new Size(80, 30);
            btnBrowseSync.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseSync.Click += btnBrowseSync_Click;

            // Backup folder (#4: default with "Use default" checkbox)
            chkUseDefaultBackup.Text = "Use default backup location (Pictures\\LightroomBackups)";
            chkUseDefaultBackup.ForeColor = textPrimary;
            chkUseDefaultBackup.BackColor = bgDark;
            chkUseDefaultBackup.Font = new Font("Segoe UI", 9F);
            chkUseDefaultBackup.Location = new Point(spacing, 156);
            chkUseDefaultBackup.AutoSize = true;
            chkUseDefaultBackup.CheckedChanged += chkUseDefaultBackup_CheckedChanged;

            lblBackupFolder.Text = "Custom backup folder";
            lblBackupFolder.ForeColor = textMuted;
            lblBackupFolder.Font = new Font("Segoe UI", 9F);
            lblBackupFolder.Location = new Point(spacing, 188);
            lblBackupFolder.AutoSize = true;

            txtBackupFolder.BackColor = inputBg;
            txtBackupFolder.ForeColor = textPrimary;
            txtBackupFolder.BorderStyle = BorderStyle.FixedSingle;
            txtBackupFolder.Font = new Font("Consolas", 9.5F);
            txtBackupFolder.Location = new Point(spacing, 212);
            txtBackupFolder.Size = new Size(420, 28);
            txtBackupFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBackupFolder.TextChanged += txtBackupFolder_TextChanged;

            btnBrowseBackup.Text = "Browse";
            btnBrowseBackup.FlatStyle = FlatStyle.Flat;
            btnBrowseBackup.BackColor = inputBg;
            btnBrowseBackup.ForeColor = textPrimary;
            btnBrowseBackup.Font = new Font("Segoe UI", 9F);
            btnBrowseBackup.FlatAppearance.BorderColor = inputBorder;
            btnBrowseBackup.Location = new Point(448, 210);
            btnBrowseBackup.Size = new Size(80, 30);
            btnBrowseBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseBackup.Click += btnBrowseBackup_Click;

            // Buttons
            btnSave.Text = "Save";
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.BackColor = accent;
            btnSave.ForeColor = Color.White;
            btnSave.Font = new Font("Segoe UI Semibold", 9.5F);
            btnSave.FlatAppearance.BorderSize = 0;
            // Log folder
            chkUseDefaultLogFolder = new CheckBox();
            chkUseDefaultLogFolder.Text = "Use default log folder (sync folder\\Logs)";
            chkUseDefaultLogFolder.ForeColor = textPrimary;
            chkUseDefaultLogFolder.BackColor = bgDark;
            chkUseDefaultLogFolder.Font = new Font("Segoe UI", 9F);
            chkUseDefaultLogFolder.Location = new Point(spacing, 248);
            chkUseDefaultLogFolder.AutoSize = true;
            chkUseDefaultLogFolder.CheckedChanged += chkUseDefaultLogFolder_CheckedChanged;

            lblLogFolder = new Label();
            lblLogFolder.Text = "Custom log folder";
            lblLogFolder.ForeColor = textMuted;
            lblLogFolder.Font = new Font("Segoe UI", 9F);
            lblLogFolder.Location = new Point(spacing, 280);
            lblLogFolder.AutoSize = true;

            txtLogFolder = new TextBox();
            txtLogFolder.BackColor = inputBg;
            txtLogFolder.ForeColor = textPrimary;
            txtLogFolder.BorderStyle = BorderStyle.FixedSingle;
            txtLogFolder.Font = new Font("Consolas", 9.5F);
            txtLogFolder.Location = new Point(spacing, 304);
            txtLogFolder.Size = new Size(420, 28);
            txtLogFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnBrowseLogFolder = new Button();
            btnBrowseLogFolder.Text = "Browse";
            btnBrowseLogFolder.FlatStyle = FlatStyle.Flat;
            btnBrowseLogFolder.BackColor = inputBg;
            btnBrowseLogFolder.ForeColor = textPrimary;
            btnBrowseLogFolder.Font = new Font("Segoe UI", 9F);
            btnBrowseLogFolder.FlatAppearance.BorderColor = inputBorder;
            btnBrowseLogFolder.Location = new Point(448, 302);
            btnBrowseLogFolder.Size = new Size(80, 30);
            btnBrowseLogFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseLogFolder.Click += btnBrowseLogFolder_Click;

            // Lightroom exe (optional)
            lblLightroomPath = new Label();
            lblLightroomPath.Text = "Lightroom Classic exe (optional; for Launch LR button)";
            lblLightroomPath.ForeColor = textMuted;
            lblLightroomPath.Font = new Font("Segoe UI", 9F);
            lblLightroomPath.Location = new Point(spacing, 344);
            lblLightroomPath.AutoSize = true;

            txtLightroomPath = new TextBox();
            txtLightroomPath.BackColor = inputBg;
            txtLightroomPath.ForeColor = textPrimary;
            txtLightroomPath.BorderStyle = BorderStyle.FixedSingle;
            txtLightroomPath.Font = new Font("Consolas", 9.5F);
            txtLightroomPath.Location = new Point(spacing, 368);
            txtLightroomPath.Size = new Size(420, 28);
            txtLightroomPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtLightroomPath.TextChanged += txtLightroomPath_TextChanged;

            btnBrowseLightroom = new Button();
            btnBrowseLightroom.Text = "Browse";
            btnBrowseLightroom.FlatStyle = FlatStyle.Flat;
            btnBrowseLightroom.BackColor = inputBg;
            btnBrowseLightroom.ForeColor = textPrimary;
            btnBrowseLightroom.Font = new Font("Segoe UI", 9F);
            btnBrowseLightroom.FlatAppearance.BorderColor = inputBorder;
            btnBrowseLightroom.Location = new Point(448, 366);
            btnBrowseLightroom.Size = new Size(80, 30);
            btnBrowseLightroom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseLightroom.Click += btnBrowseLightroom_Click;

            // Buttons
            btnSave.Size = new Size(100, 36);
            btnSave.Location = new Point(348, 412);
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Click += btnSave_Click;

            btnCancel.Text = "Cancel";
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.BackColor = inputBg;
            btnCancel.ForeColor = textPrimary;
            btnCancel.Font = new Font("Segoe UI", 9.5F);
            btnCancel.FlatAppearance.BorderColor = inputBorder;
            btnCancel.Size = new Size(100, 36);
            btnCancel.Location = new Point(238, 412);
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Click += btnCancel_Click;

            // Form
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = bgDark;
            ClientSize = new Size(548, 469);
            Controls.Add(lblLocalFolder);
            Controls.Add(txtLocalFolder);
            Controls.Add(btnBrowseLocal);
            Controls.Add(lblSyncFolder);
            Controls.Add(txtSyncFolder);
            Controls.Add(btnBrowseSync);
            Controls.Add(chkUseDefaultBackup);
            Controls.Add(lblBackupFolder);
            Controls.Add(txtBackupFolder);
            Controls.Add(btnBrowseBackup);
            Controls.Add(chkUseDefaultLogFolder);
            Controls.Add(lblLogFolder);
            Controls.Add(txtLogFolder);
            Controls.Add(btnBrowseLogFolder);
            Controls.Add(lblLightroomPath);
            Controls.Add(txtLightroomPath);
            Controls.Add(btnBrowseLightroom);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";

            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblLocalFolder;
        private TextBox txtLocalFolder;
        private Button btnBrowseLocal;
        private Label lblSyncFolder;
        private TextBox txtSyncFolder;
        private Button btnBrowseSync;
        private CheckBox chkUseDefaultBackup;
        private Label lblBackupFolder;
        private TextBox txtBackupFolder;
        private Button btnBrowseBackup;
        private CheckBox chkUseDefaultLogFolder;
        private Label lblLogFolder;
        private TextBox txtLogFolder;
        private Button btnBrowseLogFolder;
        private Label lblLightroomPath;
        private TextBox txtLightroomPath;
        private Button btnBrowseLightroom;
        private Button btnSave;
        private Button btnCancel;
    }
}
