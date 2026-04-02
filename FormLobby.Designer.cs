namespace CaroGame
{
    partial class FormLobby
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnPlay;

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
            this.btnPlay = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // btnPlay
            this.btnPlay.Location = new System.Drawing.Point(50, 50);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(150, 50);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "Play Caro";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlay_Click);

            // FormLobby
            this.ClientSize = new System.Drawing.Size(250, 200);
            this.Controls.Add(this.btnPlay);
            this.Name = "FormLobby";
            this.Text = "Lobby";

            this.ResumeLayout(false);
        }
    }
}