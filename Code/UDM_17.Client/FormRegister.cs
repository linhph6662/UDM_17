using System;
using System.Windows.Forms;

namespace UDM_17.Client
{
    public partial class FormRegister : Form
    {
        public string Username => txtUsername.Text.Trim();
        public string DisplayNameText => txtDisplayName.Text.Trim();
        public string Password => txtPassword.Text;

        public FormRegister()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(DisplayNameText) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Xác nhận mật khẩu chưa khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
