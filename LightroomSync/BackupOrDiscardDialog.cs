namespace LightroomSync
{
    /// <summary>
    /// Dialog shown when a newer catalog is available - user chooses to backup current catalog or discard it.
    /// </summary>
    public enum BackupOrDiscardChoice
    {
        Backup,
        Discard,
        Cancel
    }

    public partial class BackupOrDiscardDialog : Form
    {
        public BackupOrDiscardChoice Choice { get; private set; } = BackupOrDiscardChoice.Cancel;

        public BackupOrDiscardDialog(string catalogName)
        {
            InitializeComponent();
            labelMessage.Text = $"A newer version of catalog \"{catalogName}\" is available.\n\n" +
                "What would you like to do with your current catalog?";
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            Choice = BackupOrDiscardChoice.Backup;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonDiscard_Click(object sender, EventArgs e)
        {
            Choice = BackupOrDiscardChoice.Discard;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Choice = BackupOrDiscardChoice.Cancel;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
