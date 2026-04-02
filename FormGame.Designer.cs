namespace CaroGame
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
            SuspendLayout();
            // 
            // FormGame
            // 
            ClientSize = new Size(500, 500);
            Name = "FormGame";
            Text = "Caro Game";
            Load += FormGame_Load;
            ResumeLayout(false);
        }
    }
}