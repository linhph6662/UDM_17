namespace UDM_17.Client;

partial class FormLobby
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        lblTitle = new Label();
        picUserAvatar = new PictureBox();
        lblUserWelcome = new Label();
        lblUserInfo = new Label();
        pnlLeft = new Panel();
        btnProfile = new Button();
        btnRanking = new Button();
        btnCreateRoom = new Button();
        btnQuickMatch = new Button();
        pnlRight = new Panel();
        lblRoomHint = new Label();
        lblRooms = new Label();
        lstOpenRooms = new ListBox();
        lblJoin = new Label();
        txtRoomId = new TextBox();
        btnJoinRoom = new Button();
        btnLogin = new Button();
        btnRegister = new Button();
        btnLogout = new Button();
        lblStatus = new Label();
        pnlWaiting = new Panel();
        btnCancelWait = new Button();
        progressWaiting = new ProgressBar();
        lblWaitingTitle = new Label();
        ((System.ComponentModel.ISupportInitialize)picUserAvatar).BeginInit();
        pnlLeft.SuspendLayout();
        pnlRight.SuspendLayout();
        pnlWaiting.SuspendLayout();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(26, 127, 216);
        lblTitle.Location = new Point(43, 24);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(362, 62);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Sảnh Chơi Caro";
        // 
        // picUserAvatar
        // 
        picUserAvatar.BackColor = Color.White;
        picUserAvatar.Location = new Point(880, 13);
        picUserAvatar.Margin = new Padding(3, 4, 3, 4);
        picUserAvatar.Name = "picUserAvatar";
        picUserAvatar.Size = new Size(91, 107);
        picUserAvatar.SizeMode = PictureBoxSizeMode.Zoom;
        picUserAvatar.TabIndex = 3;
        picUserAvatar.TabStop = false;
        picUserAvatar.Visible = false;
        // 
        // lblUserWelcome
        // 
        lblUserWelcome.AutoSize = true;
        lblUserWelcome.Font = new Font("Segoe UI", 11F);
        lblUserWelcome.ForeColor = Color.FromArgb(33, 37, 41);
        lblUserWelcome.Location = new Point(899, 125);
        lblUserWelcome.Name = "lblUserWelcome";
        lblUserWelcome.Size = new Size(60, 25);
        lblUserWelcome.TabIndex = 4;
        lblUserWelcome.Text = "Guest";
        lblUserWelcome.Visible = false;
        lblUserWelcome.Click += lblUserWelcome_Click;
        // 
        // lblUserInfo
        // 
        lblUserInfo.AutoSize = true;
        lblUserInfo.Font = new Font("Segoe UI", 10F);
        lblUserInfo.ForeColor = Color.FromArgb(108, 117, 125);
        lblUserInfo.Location = new Point(840, 150);
        lblUserInfo.Name = "lblUserInfo";
        lblUserInfo.Size = new Size(183, 23);
        lblUserInfo.TabIndex = 5;
        lblUserInfo.Text = "Đăng nhập để bắt đầu";
        lblUserInfo.TextAlign = ContentAlignment.TopRight;
        // 
        // pnlLeft
        // 
        pnlLeft.BackColor = Color.White;
        pnlLeft.Controls.Add(btnProfile);
        pnlLeft.Controls.Add(btnRanking);
        pnlLeft.Controls.Add(btnCreateRoom);
        pnlLeft.Controls.Add(btnQuickMatch);
        pnlLeft.Location = new Point(43, 184);
        pnlLeft.Margin = new Padding(3, 4, 3, 4);
        pnlLeft.Name = "pnlLeft";
        pnlLeft.Size = new Size(480, 667);
        pnlLeft.TabIndex = 2;
        // 
        // btnProfile
        // 
        btnProfile.BackColor = Color.FromArgb(26, 127, 216);
        btnProfile.Cursor = Cursors.Hand;
        btnProfile.FlatAppearance.BorderSize = 0;
        btnProfile.FlatStyle = FlatStyle.Flat;
        btnProfile.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnProfile.ForeColor = Color.White;
        btnProfile.Location = new Point(23, 320);
        btnProfile.Margin = new Padding(3, 4, 3, 4);
        btnProfile.Name = "btnProfile";
        btnProfile.Size = new Size(434, 67);
        btnProfile.TabIndex = 3;
        btnProfile.Text = "👤 Hồ Sơ";
        btnProfile.UseVisualStyleBackColor = false;
        btnProfile.Click += btnProfile_Click;
        // 
        // btnRanking
        // 
        btnRanking.BackColor = Color.FromArgb(26, 127, 216);
        btnRanking.Cursor = Cursors.Hand;
        btnRanking.FlatAppearance.BorderSize = 0;
        btnRanking.FlatStyle = FlatStyle.Flat;
        btnRanking.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnRanking.ForeColor = Color.White;
        btnRanking.Location = new Point(23, 227);
        btnRanking.Margin = new Padding(3, 4, 3, 4);
        btnRanking.Name = "btnRanking";
        btnRanking.Size = new Size(434, 67);
        btnRanking.TabIndex = 2;
        btnRanking.Text = "🏆 Bảng Xếp Hạng";
        btnRanking.UseVisualStyleBackColor = false;
        btnRanking.Click += btnRanking_Click;
        // 
        // btnCreateRoom
        // 
        btnCreateRoom.BackColor = Color.FromArgb(26, 127, 216);
        btnCreateRoom.Cursor = Cursors.Hand;
        btnCreateRoom.FlatAppearance.BorderSize = 0;
        btnCreateRoom.FlatStyle = FlatStyle.Flat;
        btnCreateRoom.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnCreateRoom.ForeColor = Color.White;
        btnCreateRoom.Location = new Point(23, 133);
        btnCreateRoom.Margin = new Padding(3, 4, 3, 4);
        btnCreateRoom.Name = "btnCreateRoom";
        btnCreateRoom.Size = new Size(434, 67);
        btnCreateRoom.TabIndex = 1;
        btnCreateRoom.Text = "➕ Tạo Phòng";
        btnCreateRoom.UseVisualStyleBackColor = false;
        btnCreateRoom.Click += btnCreateRoom_Click;
        // 
        // btnQuickMatch
        // 
        btnQuickMatch.BackColor = Color.FromArgb(26, 127, 216);
        btnQuickMatch.Cursor = Cursors.Hand;
        btnQuickMatch.FlatAppearance.BorderSize = 0;
        btnQuickMatch.FlatStyle = FlatStyle.Flat;
        btnQuickMatch.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        btnQuickMatch.ForeColor = Color.White;
        btnQuickMatch.Location = new Point(23, 40);
        btnQuickMatch.Margin = new Padding(3, 4, 3, 4);
        btnQuickMatch.Name = "btnQuickMatch";
        btnQuickMatch.Size = new Size(434, 73);
        btnQuickMatch.TabIndex = 0;
        btnQuickMatch.Text = "🎮 Ghép Nhanh";
        btnQuickMatch.UseVisualStyleBackColor = false;
        btnQuickMatch.Click += btnQuickMatch_Click;
        // 
        // pnlRight
        // 
        pnlRight.BackColor = Color.White;
        pnlRight.Controls.Add(lblRoomHint);
        pnlRight.Controls.Add(lblRooms);
        pnlRight.Controls.Add(lstOpenRooms);
        pnlRight.Controls.Add(lblJoin);
        pnlRight.Controls.Add(txtRoomId);
        pnlRight.Controls.Add(btnJoinRoom);
        pnlRight.Location = new Point(570, 184);
        pnlRight.Margin = new Padding(3, 4, 3, 4);
        pnlRight.Name = "pnlRight";
        pnlRight.Size = new Size(497, 667);
        pnlRight.TabIndex = 3;
        // 
        // lblRoomHint
        // 
        lblRoomHint.BackColor = Color.FromArgb(248, 249, 250);
        lblRoomHint.Font = new Font("Segoe UI", 11F);
        lblRoomHint.ForeColor = Color.FromArgb(108, 117, 125);
        lblRoomHint.Location = new Point(26, 83);
        lblRoomHint.Name = "lblRoomHint";
        lblRoomHint.Size = new Size(446, 64);
        lblRoomHint.TabIndex = 1;
        lblRoomHint.Text = "Chưa có phòng nào đang mở";
        lblRoomHint.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblRooms
        // 
        lblRooms.AutoSize = true;
        lblRooms.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblRooms.ForeColor = Color.FromArgb(33, 37, 41);
        lblRooms.Location = new Point(21, 21);
        lblRooms.Name = "lblRooms";
        lblRooms.Size = new Size(241, 37);
        lblRooms.TabIndex = 0;
        lblRooms.Text = "Phòng Hợp Lệ (0)";
        // 
        // lstOpenRooms
        // 
        lstOpenRooms.Font = new Font("Segoe UI", 10F);
        lstOpenRooms.FormattingEnabled = true;
        lstOpenRooms.Location = new Point(26, 160);
        lstOpenRooms.Margin = new Padding(3, 4, 3, 4);
        lstOpenRooms.Name = "lstOpenRooms";
        lstOpenRooms.Size = new Size(445, 280);
        lstOpenRooms.TabIndex = 2;
        lstOpenRooms.SelectedIndexChanged += lstOpenRooms_SelectedIndexChanged;
        // 
        // lblJoin
        // 
        lblJoin.AutoSize = true;
        lblJoin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblJoin.ForeColor = Color.FromArgb(33, 37, 41);
        lblJoin.Location = new Point(24, 473);
        lblJoin.Name = "lblJoin";
        lblJoin.Size = new Size(149, 25);
        lblJoin.TabIndex = 2;
        lblJoin.Text = "Nhập ID Phòng";
        // 
        // txtRoomId
        // 
        txtRoomId.BorderStyle = BorderStyle.FixedSingle;
        txtRoomId.Font = new Font("Segoe UI", 11F);
        txtRoomId.Location = new Point(26, 509);
        txtRoomId.Margin = new Padding(3, 4, 3, 4);
        txtRoomId.Name = "txtRoomId";
        txtRoomId.PlaceholderText = "VD: 12345678";
        txtRoomId.Size = new Size(303, 32);
        txtRoomId.TabIndex = 3;
        // 
        // btnJoinRoom
        // 
        btnJoinRoom.BackColor = Color.FromArgb(26, 127, 216);
        btnJoinRoom.Cursor = Cursors.Hand;
        btnJoinRoom.FlatAppearance.BorderSize = 0;
        btnJoinRoom.FlatStyle = FlatStyle.Flat;
        btnJoinRoom.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnJoinRoom.ForeColor = Color.White;
        btnJoinRoom.Location = new Point(341, 501);
        btnJoinRoom.Margin = new Padding(3, 4, 3, 4);
        btnJoinRoom.Name = "btnJoinRoom";
        btnJoinRoom.Size = new Size(131, 53);
        btnJoinRoom.TabIndex = 4;
        btnJoinRoom.Text = "Vào";
        btnJoinRoom.UseVisualStyleBackColor = false;
        btnJoinRoom.Click += btnJoinRoom_Click;
        // 
        // btnLogin
        // 
        btnLogin.BackColor = Color.FromArgb(26, 127, 216);
        btnLogin.Cursor = Cursors.Hand;
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnLogin.ForeColor = Color.White;
        btnLogin.Location = new Point(884, 39);
        btnLogin.Margin = new Padding(3, 4, 3, 4);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(87, 47);
        btnLogin.TabIndex = 4;
        btnLogin.Text = "Login";
        btnLogin.UseVisualStyleBackColor = false;
        btnLogin.Click += btnLogin_Click;
        // 
        // btnRegister
        // 
        btnRegister.BackColor = Color.FromArgb(108, 117, 125);
        btnRegister.Cursor = Cursors.Hand;
        btnRegister.FlatAppearance.BorderSize = 0;
        btnRegister.FlatStyle = FlatStyle.Flat;
        btnRegister.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnRegister.ForeColor = Color.White;
        btnRegister.Location = new Point(990, 40);
        btnRegister.Margin = new Padding(3, 4, 3, 4);
        btnRegister.Name = "btnRegister";
        btnRegister.Size = new Size(87, 47);
        btnRegister.TabIndex = 5;
        btnRegister.Text = "Đăng Ký";
        btnRegister.UseVisualStyleBackColor = false;
        btnRegister.Click += btnRegister_Click;
        // 
        // btnLogout
        // 
        btnLogout.BackColor = Color.FromArgb(211, 55, 75);
        btnLogout.Cursor = Cursors.Hand;
        btnLogout.FlatAppearance.BorderSize = 0;
        btnLogout.FlatStyle = FlatStyle.Flat;
        btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnLogout.ForeColor = Color.White;
        btnLogout.Location = new Point(990, 40);
        btnLogout.Margin = new Padding(3, 4, 3, 4);
        btnLogout.Name = "btnLogout";
        btnLogout.Size = new Size(87, 47);
        btnLogout.TabIndex = 6;
        btnLogout.Text = "Logout";
        btnLogout.UseVisualStyleBackColor = false;
        btnLogout.Visible = false;
        btnLogout.Click += btnLogout_Click;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 9F);
        lblStatus.ForeColor = Color.FromArgb(108, 117, 125);
        lblStatus.Location = new Point(43, 877);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(101, 20);
        lblStatus.TabIndex = 6;
        lblStatus.Text = "Chưa kết nối...";
        // 
        // pnlWaiting
        // 
        pnlWaiting.BackColor = Color.White;
        pnlWaiting.Controls.Add(btnCancelWait);
        pnlWaiting.Controls.Add(progressWaiting);
        pnlWaiting.Controls.Add(lblWaitingTitle);
        pnlWaiting.Location = new Point(354, 301);
        pnlWaiting.Margin = new Padding(3, 4, 3, 4);
        pnlWaiting.Name = "pnlWaiting";
        pnlWaiting.Size = new Size(423, 307);
        pnlWaiting.TabIndex = 7;
        pnlWaiting.Visible = false;
        // 
        // btnCancelWait
        // 
        btnCancelWait.BackColor = Color.FromArgb(211, 55, 75);
        btnCancelWait.Cursor = Cursors.Hand;
        btnCancelWait.FlatAppearance.BorderSize = 0;
        btnCancelWait.FlatStyle = FlatStyle.Flat;
        btnCancelWait.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnCancelWait.ForeColor = Color.White;
        btnCancelWait.Location = new Point(112, 201);
        btnCancelWait.Margin = new Padding(3, 4, 3, 4);
        btnCancelWait.Name = "btnCancelWait";
        btnCancelWait.Size = new Size(198, 61);
        btnCancelWait.TabIndex = 2;
        btnCancelWait.Text = "Hủy";
        btnCancelWait.UseVisualStyleBackColor = false;
        btnCancelWait.Click += btnCancelWait_Click;
        // 
        // progressWaiting
        // 
        progressWaiting.Location = new Point(55, 120);
        progressWaiting.Margin = new Padding(3, 4, 3, 4);
        progressWaiting.MarqueeAnimationSpeed = 25;
        progressWaiting.Name = "progressWaiting";
        progressWaiting.Size = new Size(309, 32);
        progressWaiting.Style = ProgressBarStyle.Marquee;
        progressWaiting.TabIndex = 1;
        // 
        // lblWaitingTitle
        // 
        lblWaitingTitle.AutoSize = true;
        lblWaitingTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblWaitingTitle.ForeColor = Color.FromArgb(33, 37, 41);
        lblWaitingTitle.Location = new Point(78, 44);
        lblWaitingTitle.Name = "lblWaitingTitle";
        lblWaitingTitle.Size = new Size(256, 37);
        lblWaitingTitle.TabIndex = 0;
        lblWaitingTitle.Text = "Đang tìm đối thủ...";
        // 
        // FormLobby
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(248, 249, 250);
        ClientSize = new Size(1120, 920);
        Controls.Add(pnlWaiting);
        Controls.Add(lblStatus);
        Controls.Add(btnLogout);
        Controls.Add(btnRegister);
        Controls.Add(btnLogin);
        Controls.Add(pnlRight);
        Controls.Add(pnlLeft);
        Controls.Add(lblUserInfo);
        Controls.Add(lblUserWelcome);
        Controls.Add(picUserAvatar);
        Controls.Add(lblTitle);
        Margin = new Padding(3, 4, 3, 4);
        Name = "FormLobby";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sảnh chơi - UDM_17";
        Load += FormLobby_Load;
        ((System.ComponentModel.ISupportInitialize)picUserAvatar).EndInit();
        pnlLeft.ResumeLayout(false);
        pnlRight.ResumeLayout(false);
        pnlRight.PerformLayout();
        pnlWaiting.ResumeLayout(false);
        pnlWaiting.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.PictureBox picUserAvatar;
    private System.Windows.Forms.Label lblUserWelcome;
    private System.Windows.Forms.Panel pnlLeft;
    private System.Windows.Forms.Button btnCreateRoom;
    private System.Windows.Forms.Button btnRanking;
    private System.Windows.Forms.Button btnProfile;
    private System.Windows.Forms.Button btnLogin;
    private System.Windows.Forms.Button btnRegister;
    private System.Windows.Forms.Button btnQuickMatch;
    private System.Windows.Forms.Panel pnlRight;
    private System.Windows.Forms.Label lblRooms;
    private System.Windows.Forms.Label lblRoomHint;
    private System.Windows.Forms.ListBox lstOpenRooms;
    private System.Windows.Forms.Label lblJoin;
    private System.Windows.Forms.TextBox txtRoomId;
    private System.Windows.Forms.Button btnJoinRoom;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.Label lblUserInfo;
    private System.Windows.Forms.Button btnLogout;
    private System.Windows.Forms.Panel pnlWaiting;
    private System.Windows.Forms.Label lblWaitingTitle;
    private System.Windows.Forms.ProgressBar progressWaiting;
    private System.Windows.Forms.Button btnCancelWait;
}
