namespace UDM_17.Client
{
    partial class FormProfile
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
            lblTitle = new Label();
            picAvatar = new PictureBox();
            btnChangeAvatar = new Button();
            lblUserId = new Label();
            lblUserIdValue = new Label();
            lblUsername = new Label();
            lblUsernameValue = new Label();
            lblDisplayName = new Label();
            lblDisplayNameValue = new Label();
            lblScore = new Label();
            lblScoreValue = new Label();
            btnRefresh = new Button();
            lblUpdate = new Label();
            dgvHistory = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(26, 127, 216);
            lblTitle.Location = new Point(32, 24);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(447, 62);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Hồ Sơ Người Dùng";
            // 
            // picAvatar
            // 
            picAvatar.Location = new Point(32, 113);
            picAvatar.Margin = new Padding(3, 4, 3, 4);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new Size(183, 213);
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.TabIndex = 1;
            picAvatar.TabStop = false;
            // 
            // btnChangeAvatar
            // 
            btnChangeAvatar.BackColor = Color.FromArgb(108, 117, 125);
            btnChangeAvatar.Cursor = Cursors.Hand;
            btnChangeAvatar.FlatAppearance.BorderSize = 0;
            btnChangeAvatar.FlatStyle = FlatStyle.Flat;
            btnChangeAvatar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnChangeAvatar.ForeColor = Color.White;
            btnChangeAvatar.Location = new Point(32, 344);
            btnChangeAvatar.Margin = new Padding(3, 4, 3, 4);
            btnChangeAvatar.Name = "btnChangeAvatar";
            btnChangeAvatar.Size = new Size(183, 53);
            btnChangeAvatar.TabIndex = 2;
            btnChangeAvatar.Text = "📄 Thay Đổi";
            btnChangeAvatar.UseVisualStyleBackColor = false;
            btnChangeAvatar.Click += btnChangeAvatar_Click;
            // 
            // lblUserId
            // 
            lblUserId.AutoSize = true;
            lblUserId.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblUserId.ForeColor = Color.FromArgb(33, 37, 41);
            lblUserId.Location = new Point(247, 132);
            lblUserId.Name = "lblUserId";
            lblUserId.Size = new Size(104, 28);
            lblUserId.TabIndex = 3;
            lblUserId.Text = "ID Người:";
            // 
            // lblUserIdValue
            // 
            lblUserIdValue.AutoSize = true;
            lblUserIdValue.Font = new Font("Segoe UI", 11F);
            lblUserIdValue.ForeColor = Color.FromArgb(108, 117, 125);
            lblUserIdValue.Location = new Point(347, 135);
            lblUserIdValue.Name = "lblUserIdValue";
            lblUserIdValue.Size = new Size(162, 25);
            lblUserIdValue.TabIndex = 4;
            lblUserIdValue.Text = "202600000000000";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(33, 37, 41);
            lblUsername.Location = new Point(247, 180);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(157, 28);
            lblUsername.TabIndex = 5;
            lblUsername.Text = "Tên đăng nhập:";
            // 
            // lblUsernameValue
            // 
            lblUsernameValue.AutoSize = true;
            lblUsernameValue.Font = new Font("Segoe UI", 11F);
            lblUsernameValue.ForeColor = Color.FromArgb(108, 117, 125);
            lblUsernameValue.Location = new Point(410, 183);
            lblUsernameValue.Name = "lblUsernameValue";
            lblUsernameValue.Size = new Size(44, 25);
            lblUsernameValue.TabIndex = 6;
            lblUsernameValue.Text = "linh";
            // 
            // lblDisplayName
            // 
            lblDisplayName.AutoSize = true;
            lblDisplayName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDisplayName.ForeColor = Color.FromArgb(33, 37, 41);
            lblDisplayName.Location = new Point(247, 228);
            lblDisplayName.Name = "lblDisplayName";
            lblDisplayName.Size = new Size(136, 28);
            lblDisplayName.TabIndex = 7;
            lblDisplayName.Text = "Tên Hiển Thị:";
            // 
            // lblDisplayNameValue
            // 
            lblDisplayNameValue.AutoSize = true;
            lblDisplayNameValue.Font = new Font("Segoe UI", 11F);
            lblDisplayNameValue.ForeColor = Color.FromArgb(108, 117, 125);
            lblDisplayNameValue.Location = new Point(389, 231);
            lblDisplayNameValue.Name = "lblDisplayNameValue";
            lblDisplayNameValue.Size = new Size(92, 25);
            lblDisplayNameValue.TabIndex = 8;
            lblDisplayNameValue.Text = "Hoai Linh";
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblScore.ForeColor = Color.FromArgb(33, 37, 41);
            lblScore.Location = new Point(247, 276);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(73, 28);
            lblScore.TabIndex = 9;
            lblScore.Text = " Điểm:";
            // 
            // lblScoreValue
            // 
            lblScoreValue.AutoSize = true;
            lblScoreValue.Font = new Font("Segoe UI", 11F);
            lblScoreValue.ForeColor = Color.FromArgb(108, 117, 125);
            lblScoreValue.Location = new Point(326, 279);
            lblScoreValue.Name = "lblScoreValue";
            lblScoreValue.Size = new Size(32, 25);
            lblScoreValue.TabIndex = 10;
            lblScoreValue.Text = "40";
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(26, 127, 216);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(632, 118);
            btnRefresh.Margin = new Padding(3, 4, 3, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(171, 56);
            btnRefresh.TabIndex = 11;
            btnRefresh.Text = "🔄 Làm Mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblUpdate
            // 
            lblUpdate.AutoSize = true;
            lblUpdate.Font = new Font("Segoe UI", 10F);
            lblUpdate.ForeColor = Color.FromArgb(108, 117, 125);
            lblUpdate.Location = new Point(247, 331);
            lblUpdate.Name = "lblUpdate";
            lblUpdate.Size = new Size(193, 23);
            lblUpdate.TabIndex = 12;
            lblUpdate.Text = "Hồ sơ đã được cập nhật";
            // 
            // dgvHistory
            // 
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistory.BackgroundColor = Color.White;
            dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistory.Location = new Point(35, 409);
            dgvHistory.Margin = new Padding(3, 4, 3, 4);
            dgvHistory.Name = "dgvHistory";
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.RowHeadersWidth = 51;
            dgvHistory.Size = new Size(799, 329);
            dgvHistory.TabIndex = 13;
            // 
            // FormProfile
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(869, 776);
            Controls.Add(dgvHistory);
            Controls.Add(lblUpdate);
            Controls.Add(btnRefresh);
            Controls.Add(lblScoreValue);
            Controls.Add(lblScore);
            Controls.Add(lblDisplayNameValue);
            Controls.Add(lblDisplayName);
            Controls.Add(lblUsernameValue);
            Controls.Add(lblUsername);
            Controls.Add(lblUserIdValue);
            Controls.Add(lblUserId);
            Controls.Add(btnChangeAvatar);
            Controls.Add(picAvatar);
            Controls.Add(lblTitle);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormProfile";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Profile";
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Button btnChangeAvatar;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.Label lblUserIdValue;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblUsernameValue;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.Label lblDisplayNameValue;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblScoreValue;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.DataGridView dgvHistory;
    }
}
