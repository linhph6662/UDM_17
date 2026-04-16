using System;
using System.Drawing;
using System.Windows.Forms;

namespace UDM_17.Client
{
    public partial class FormProfile : Form
    {
        public FormProfile()
        {
            InitializeComponent();
            BuildHistoryGrid();
        }

        public void LoadProfile(string userId, string username, string displayName, int score, Image? avatar)
        {
            lblUserIdValue.Text = string.IsNullOrEmpty(userId) ? "-" : userId;
            lblUsernameValue.Text = username;
            lblDisplayNameValue.Text = displayName;
            lblScoreValue.Text = score.ToString();
            picAvatar.Image = avatar ?? BuildInitialAvatar(displayName);
            LoadEmptyHistory();
        }

        private void BuildHistoryGrid()
        {
            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.Columns.Clear();
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thời gian", DataPropertyName = "Time", Width = 180 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đối thủ", DataPropertyName = "Opponent", Width = 160 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kết quả", DataPropertyName = "Result", Width = 100 });
        }

        private void LoadEmptyHistory()
        {
            dgvHistory.DataSource = Array.Empty<object>();
        }

        private Image BuildInitialAvatar(string displayName)
        {
            Bitmap bitmap = new Bitmap(150, 150);
            using Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.FromArgb(188, 179, 225));

            string initials = "HL";
            if (!string.IsNullOrWhiteSpace(displayName))
            {
                var parts = displayName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                {
                    initials = parts[0][0].ToString().ToUpperInvariant();
                }
                else
                {
                    initials = string.Concat(parts[0][0], parts[^1][0]).ToUpperInvariant();
                }
            }

            using Font f = new Font("Segoe UI", 34, FontStyle.Bold);
            SizeF size = g.MeasureString(initials, f);
            g.DrawString(initials, f, Brushes.Black, (bitmap.Width - size.Width) / 2f, (bitmap.Height - size.Height) / 2f);
            return bitmap;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lblUpdate.Text = "Profile da duoc cap nhat.";
            LoadEmptyHistory();
        }

        private void btnChangeAvatar_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                picAvatar.Image = Image.FromFile(ofd.FileName);
            }
        }
    }
}
