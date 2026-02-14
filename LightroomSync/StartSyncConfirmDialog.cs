namespace LightroomSync
{
    internal partial class StartSyncConfirmDialog : Form
    {
        public bool DontShowAgain => chkDontShowAgain.Checked;

        public StartSyncConfirmDialog()
        {
            InitializeComponent();
        }
    }
}
