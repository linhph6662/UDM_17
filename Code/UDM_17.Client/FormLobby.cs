using System;
using System.Windows.Forms;

namespace UDM_17.Client
{
    public partial class FormLobby : Form
    {
        public FormLobby()
        {
            InitializeComponent();
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            FormGame game = new FormGame();
            game.Show();
            this.Hide();
        }
    }
}
