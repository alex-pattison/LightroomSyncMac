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
            labelMessage = new Label();
            buttonBackup = new Button();
            buttonDiscard = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // labelMessage
            // 
            labelMessage.AutoSize = false;
            labelMessage.BackColor = Color.FromArgb(64, 64, 64);
            labelMessage.ForeColor = Color.WhiteSmoke;
            labelMessage.Location = new Point(20, 20);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(440, 80);
            labelMessage.TabIndex = 0;
            labelMessage.Text = "A newer version of the catalog is available.";
            // 
            // buttonBackup
            // 
            buttonBackup.Location = new Point(20, 110);
            buttonBackup.Name = "buttonBackup";
            buttonBackup.Size = new Size(140, 35);
            buttonBackup.TabIndex = 1;
            buttonBackup.Text = "Backup";
            buttonBackup.UseVisualStyleBackColor = true;
            buttonBackup.Click += buttonBackup_Click;
            // 
            // buttonDiscard
            // 
            buttonDiscard.Location = new Point(170, 110);
            buttonDiscard.Name = "buttonDiscard";
            buttonDiscard.Size = new Size(140, 35);
            buttonDiscard.TabIndex = 2;
            buttonDiscard.Text = "Discard";
            buttonDiscard.UseVisualStyleBackColor = true;
            buttonDiscard.Click += buttonDiscard_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(320, 110);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(140, 35);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // BackupOrDiscardDialog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(480, 165);
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
