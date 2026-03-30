namespace UDM_17.Server
{
    partial class FormServer
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
            btnStartServer = new Button();
            lstLog = new ListBox();
            SuspendLayout();
            // 
            // btnStartServer
            // 
            btnStartServer.Dock = DockStyle.Top;
            btnStartServer.Location = new Point(0, 0);
            btnStartServer.Margin = new Padding(3, 4, 3, 4);
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new Size(452, 60);
            btnStartServer.TabIndex = 0;
            btnStartServer.Text = "BẮT ĐẦU SERVER (PORT 1234)";
            btnStartServer.UseVisualStyleBackColor = true;
            // 
            // lstLog
            // 
            lstLog.Dock = DockStyle.Fill;
            lstLog.FormattingEnabled = true;
            lstLog.Location = new Point(0, 60);
            lstLog.Margin = new Padding(3, 4, 3, 4);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(452, 421);
            lstLog.TabIndex = 1;
            // 
            // FormServer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 481);
            Controls.Add(lstLog);
            Controls.Add(btnStartServer);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormServer";
            Text = "UDM_17 - Quản lý Server";
            ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.ListBox lstLog;
    }
}