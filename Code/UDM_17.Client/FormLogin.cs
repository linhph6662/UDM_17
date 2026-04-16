using System;
using System.Windows.Forms;

namespace UDM_17.Client
{
    public partial class FormLogin : Form
    {
        public string Username => txtUsername.Text.Trim();
        public string Password => txtPassword.Text;

        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Nhập đầy đủ tên đăng nhập và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            using FormRegister register = new FormRegister();
            register.ShowDialog(this);
        }
    }
}
