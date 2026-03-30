namespace UDM_17.Client
{
    partial class FormGame
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.pnlChessBoard = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(120, 30);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "KẾT NỐI SERVER";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // pnlChessBoard
            // 
            this.pnlChessBoard.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlChessBoard.Location = new System.Drawing.Point(12, 55);
            this.pnlChessBoard.Name = "pnlChessBoard";
            this.pnlChessBoard.Size = new System.Drawing.Size(455, 455);
            this.pnlChessBoard.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(150, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(84, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Chưa kết nối...";
            // 
            // FormGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 531);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlChessBoard);
            this.Controls.Add(this.btnConnect);
            this.Name = "FormGame";
            this.Text = "UDM_17 - Caro Client";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel pnlChessBoard;
        private System.Windows.Forms.Label lblStatus;
    }
}