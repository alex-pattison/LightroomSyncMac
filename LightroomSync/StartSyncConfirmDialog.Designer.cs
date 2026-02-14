namespace LightroomSync
{
    partial class StartSyncConfirmDialog
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

            lblMessage = new Label();
            chkDontShowAgain = new CheckBox();
            btnStart = new Button();
            btnCancel = new Button();

            SuspendLayout();

            lblMessage.Text = "Start sync monitoring? This will watch for Lightroom to close and sync catalogs with the network.";
            lblMessage.ForeColor = textPrimary;
            lblMessage.Font = new Font("Segoe UI", 9.5F);
            lblMessage.Location = new Point(spacing, spacing);
            lblMessage.Size = new Size(400, 60);
            lblMessage.AutoSize = false;

            chkDontShowAgain.Text = "Don't show this again";
            chkDontShowAgain.ForeColor = textMuted;
            chkDontShowAgain.BackColor = bgDark;
            chkDontShowAgain.Font = new Font("Segoe UI", 9F);
            chkDontShowAgain.Location = new Point(spacing, 90);
            chkDontShowAgain.AutoSize = true;

            btnStart.Text = "Start Sync";
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.BackColor = accent;
            btnStart.ForeColor = Color.White;
            btnStart.Font = new Font("Segoe UI Semibold", 9.5F);
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Size = new Size(110, 36);
            btnStart.Location = new Point(314, 130);
            btnStart.DialogResult = DialogResult.OK;

            btnCancel.Text = "Cancel";
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.BackColor = inputBg;
            btnCancel.ForeColor = textPrimary;
            btnCancel.Font = new Font("Segoe UI", 9.5F);
            btnCancel.FlatAppearance.BorderColor = inputBorder;
            btnCancel.Size = new Size(100, 36);
            btnCancel.Location = new Point(208, 130);
            btnCancel.DialogResult = DialogResult.Cancel;

            AcceptButton = btnStart;
            CancelButton = btnCancel;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = bgDark;
            ClientSize = new Size(440, 185);
            Controls.Add(lblMessage);
            Controls.Add(chkDontShowAgain);
            Controls.Add(btnStart);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StartSyncConfirmDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Confirm Start";

            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblMessage;
        private CheckBox chkDontShowAgain;
        private Button btnStart;
        private Button btnCancel;
    }
}
