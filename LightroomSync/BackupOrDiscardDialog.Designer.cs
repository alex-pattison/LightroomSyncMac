namespace LightroomSync
{
    partial class BackupOrDiscardDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
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

            labelMessage = new Label();
            buttonBackup = new Button();
            buttonDiscard = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            //
            // labelMessage
            //
            labelMessage.AutoSize = false;
            labelMessage.BackColor = bgDark;
            labelMessage.ForeColor = textPrimary;
            labelMessage.Font = new Font("Segoe UI", 9.5F);
            labelMessage.Location = new Point(24, 24);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(432, 72);
            labelMessage.TabIndex = 0;
            labelMessage.Text = "A newer version of the catalog is available.";
            //
            // buttonBackup
            //
            buttonBackup.FlatStyle = FlatStyle.Flat;
            buttonBackup.BackColor = accent;
            buttonBackup.ForeColor = Color.White;
            buttonBackup.Font = new Font("Segoe UI Semibold", 9.5F);
            buttonBackup.FlatAppearance.BorderSize = 0;
            buttonBackup.Cursor = Cursors.Hand;
            buttonBackup.Location = new Point(24, 108);
            buttonBackup.Name = "buttonBackup";
            buttonBackup.Size = new Size(120, 36);
            buttonBackup.TabIndex = 1;
            buttonBackup.Text = "Backup";
            buttonBackup.Click += buttonBackup_Click;
            //
            // buttonDiscard
            //
            buttonDiscard.FlatStyle = FlatStyle.Flat;
            buttonDiscard.BackColor = inputBg;
            buttonDiscard.ForeColor = textPrimary;
            buttonDiscard.Font = new Font("Segoe UI", 9.5F);
            buttonDiscard.FlatAppearance.BorderColor = inputBorder;
            buttonDiscard.Cursor = Cursors.Hand;
            buttonDiscard.Location = new Point(156, 108);
            buttonDiscard.Name = "buttonDiscard";
            buttonDiscard.Size = new Size(120, 36);
            buttonDiscard.TabIndex = 2;
            buttonDiscard.Text = "Discard";
            buttonDiscard.Click += buttonDiscard_Click;
            //
            // buttonCancel
            //
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.BackColor = inputBg;
            buttonCancel.ForeColor = textPrimary;
            buttonCancel.Font = new Font("Segoe UI", 9.5F);
            buttonCancel.FlatAppearance.BorderColor = inputBorder;
            buttonCancel.Cursor = Cursors.Hand;
            buttonCancel.Location = new Point(288, 108);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(120, 36);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.Click += buttonCancel_Click;
            //
            // BackupOrDiscardDialog
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = bgDark;
            ClientSize = new Size(432, 168);
            Controls.Add(buttonCancel);
            Controls.Add(buttonDiscard);
            Controls.Add(buttonBackup);
            Controls.Add(labelMessage);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BackupOrDiscardDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Newer Catalog Available";
            ResumeLayout(false);
        }

        private Label labelMessage;
        private Button buttonBackup;
        private Button buttonDiscard;
        private Button buttonCancel;
    }
}
