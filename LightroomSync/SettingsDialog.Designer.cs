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
            btnSave.Size = new Size(100, 36);
            btnSave.Location = new Point(348, 260);
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Click += btnSave_Click;

            btnCancel.Text = "Cancel";
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.BackColor = inputBg;
            btnCancel.ForeColor = textPrimary;
            btnCancel.Font = new Font("Segoe UI", 9.5F);
            btnCancel.FlatAppearance.BorderColor = inputBorder;
            btnCancel.Size = new Size(100, 36);
            btnCancel.Location = new Point(238, 260);
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Click += btnCancel_Click;

            // Form
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = bgDark;
            ClientSize = new Size(548, 320);
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
        private Button btnSave;
        private Button btnCancel;
    }
}
