namespace LightroomSync
{
    /// <summary>
    /// Settings dialog for catalog paths. Config is updated on OK.
    /// </summary>
    internal partial class SettingsDialog : Form
    {
        private readonly Config _config;
        private static readonly string DefaultBackupPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Pictures", "LightroomBackups");

        public SettingsDialog(Config config)
        {
            InitializeComponent();
            _config = config;

            txtLocalFolder.Text = config.LocalFolder ?? "";
            txtSyncFolder.Text = config.NetworkFolder ?? "";
            chkUseDefaultBackup.Checked = string.IsNullOrEmpty(config.BackupFolder) ||
                string.Equals(config.BackupFolder.Trim(), DefaultBackupPath, StringComparison.OrdinalIgnoreCase);
            if (chkUseDefaultBackup.Checked)
                txtBackupFolder.Text = DefaultBackupPath;
            else
                txtBackupFolder.Text = config.BackupFolder ?? "";
            UpdateBackupControls();
        }

        private void UpdateBackupControls()
        {
            txtBackupFolder.Enabled = !chkUseDefaultBackup.Checked;
            btnBrowseBackup.Enabled = !chkUseDefaultBackup.Checked;
            if (chkUseDefaultBackup.Checked)
                txtBackupFolder.Text = DefaultBackupPath;
        }

        private void chkUseDefaultBackup_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBackupControls();
        }

        private void txtLocalFolder_TextChanged(object sender, EventArgs e)
        {
            ValidatePath(txtLocalFolder);
        }

        private void txtSyncFolder_TextChanged(object sender, EventArgs e)
        {
            ValidatePath(txtSyncFolder);
        }

        private void txtBackupFolder_TextChanged(object sender, EventArgs e)
        {
            if (chkUseDefaultBackup.Checked) return;
            if (string.IsNullOrWhiteSpace(txtBackupFolder.Text) || Directory.Exists(txtBackupFolder.Text))
                txtBackupFolder.BackColor = Color.FromArgb(43, 43, 48);
            else
                txtBackupFolder.BackColor = Color.FromArgb(80, 45, 45);
        }

        private static void ValidatePath(TextBox txt)
        {
            if (Directory.Exists(txt.Text))
                txt.BackColor = Color.FromArgb(43, 43, 48);
            else
                txt.BackColor = Color.FromArgb(80, 45, 45);
        }

        private void btnBrowseLocal_Click(object sender, EventArgs e)
        {
            if (PickFolder(txtLocalFolder)) ValidatePath(txtLocalFolder);
        }

        private void btnBrowseSync_Click(object sender, EventArgs e)
        {
            if (PickFolder(txtSyncFolder)) ValidatePath(txtSyncFolder);
        }

        private void btnBrowseBackup_Click(object sender, EventArgs e)
        {
            PickFolder(txtBackupFolder);
        }

        private static bool PickFolder(TextBox target)
        {
            using var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.SelectedPath))
            {
                target.Text = dlg.SelectedPath;
                return true;
            }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtLocalFolder.Text))
            {
                MessageBox.Show("Please select a valid local catalog folder.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocalFolder.Focus();
                return;
            }
            if (!Directory.Exists(txtSyncFolder.Text))
            {
                MessageBox.Show("Please select a valid sync folder.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSyncFolder.Focus();
                return;
            }
            if (!chkUseDefaultBackup.Checked && !string.IsNullOrWhiteSpace(txtBackupFolder.Text) && !Directory.Exists(txtBackupFolder.Text))
            {
                MessageBox.Show("Backup folder path is invalid or does not exist.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBackupFolder.Focus();
                return;
            }

            _config.LocalFolder = txtLocalFolder.Text.Trim();
            _config.NetworkFolder = txtSyncFolder.Text.Trim();
            _config.BackupFolder = chkUseDefaultBackup.Checked ? DefaultBackupPath : txtBackupFolder.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
