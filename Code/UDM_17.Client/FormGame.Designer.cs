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
            components = new System.ComponentModel.Container();
            pnlChessBoard = new Panel();
            lblStatus = new Label();
            pnlRight = new Panel();
            btnLeaveRoom = new Button();
            btnRematch = new Button();
            btnRematchAccept = new Button();
            btnRematchDecline = new Button();
            lblRematchRequest = new Label();
            btnSendChat = new Button();
            txtChatInput = new TextBox();
            rtbChat = new RichTextBox();
            lblChat = new Label();
            lblThinkingTime = new Label();
            lblOpponentDisplayName = new Label();
            lblOpponentUsername = new Label();
            lblOpponentTitle = new Label();
            picOpponentAvatar = new PictureBox();
            timerThinking = new System.Windows.Forms.Timer(components);
            pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picOpponentAvatar).BeginInit();
            SuspendLayout();
            // 
            // pnlChessBoard
            // 
            pnlChessBoard.AutoScroll = true;
            pnlChessBoard.BackColor = Color.White;
            pnlChessBoard.BorderStyle = BorderStyle.FixedSingle;
            pnlChessBoard.Location = new Point(14, 67);
            pnlChessBoard.Margin = new Padding(3, 4, 3, 4);
            pnlChessBoard.Name = "pnlChessBoard";
            pnlChessBoard.Size = new Size(771, 899);
            pnlChessBoard.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(33, 37, 41);
            lblStatus.Location = new Point(14, 21);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(251, 25);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "⚡ Trận đấu đang diến ra...";
            // 
            // pnlRight
            // 
            pnlRight.AutoScroll = true;
            pnlRight.BackColor = Color.White;
            pnlRight.BorderStyle = BorderStyle.FixedSingle;
            pnlRight.Controls.Add(btnLeaveRoom);
            pnlRight.Controls.Add(btnRematch);
            pnlRight.Controls.Add(btnRematchAccept);
            pnlRight.Controls.Add(btnRematchDecline);
            pnlRight.Controls.Add(lblRematchRequest);
            pnlRight.Controls.Add(btnSendChat);
            pnlRight.Controls.Add(txtChatInput);
            pnlRight.Controls.Add(rtbChat);
            pnlRight.Controls.Add(lblChat);
            pnlRight.Controls.Add(lblThinkingTime);
            pnlRight.Controls.Add(lblOpponentDisplayName);
            pnlRight.Controls.Add(lblOpponentUsername);
            pnlRight.Controls.Add(lblOpponentTitle);
            pnlRight.Controls.Add(picOpponentAvatar);
            pnlRight.Location = new Point(800, 67);
            pnlRight.Margin = new Padding(3, 4, 3, 4);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(434, 899);
            pnlRight.TabIndex = 3;
            // 
            // btnLeaveRoom
            // 
            btnLeaveRoom.BackColor = Color.FromArgb(220, 53, 69);
            btnLeaveRoom.Cursor = Cursors.Hand;
            btnLeaveRoom.FlatAppearance.BorderSize = 0;
            btnLeaveRoom.FlatStyle = FlatStyle.Flat;
            btnLeaveRoom.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLeaveRoom.ForeColor = Color.White;
            btnLeaveRoom.Location = new Point(18, 840);
            btnLeaveRoom.Margin = new Padding(3, 4, 3, 4);
            btnLeaveRoom.Name = "btnLeaveRoom";
            btnLeaveRoom.Size = new Size(177, 37);
            btnLeaveRoom.TabIndex = 9;
            btnLeaveRoom.Text = "✖ Rời phòng";
            btnLeaveRoom.UseVisualStyleBackColor = false;
            btnLeaveRoom.Click += btnLeaveRoom_Click;
            // 
            // btnRematch
            // 
            btnRematch.BackColor = Color.FromArgb(40, 167, 69);
            btnRematch.Cursor = Cursors.Hand;
            btnRematch.Enabled = false;
            btnRematch.FlatAppearance.BorderSize = 0;
            btnRematch.FlatStyle = FlatStyle.Flat;
            btnRematch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRematch.ForeColor = Color.White;
            btnRematch.Location = new Point(227, 840);
            btnRematch.Margin = new Padding(3, 4, 3, 4);
            btnRematch.Name = "btnRematch";
            btnRematch.Size = new Size(177, 37);
            btnRematch.TabIndex = 10;
            btnRematch.Text = "🔁 Đấu lại";
            btnRematch.UseVisualStyleBackColor = false;
            btnRematch.Click += btnRematch_Click;
            // 
            // btnRematchAccept
            // 
            btnRematchAccept.BackColor = Color.FromArgb(40, 167, 69);
            btnRematchAccept.Cursor = Cursors.Hand;
            btnRematchAccept.FlatAppearance.BorderSize = 0;
            btnRematchAccept.FlatStyle = FlatStyle.Flat;
            btnRematchAccept.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRematchAccept.ForeColor = Color.White;
            btnRematchAccept.Location = new Point(75, 690);
            btnRematchAccept.Margin = new Padding(3, 4, 3, 4);
            btnRematchAccept.Name = "btnRematchAccept";
            btnRematchAccept.Size = new Size(150, 40);
            btnRematchAccept.TabIndex = 11;
            btnRematchAccept.Text = "✓ Đồng ý";
            btnRematchAccept.UseVisualStyleBackColor = false;
            btnRematchAccept.Visible = false;
            btnRematchAccept.Click += btnRematchAccept_Click;
            // 
            // btnRematchDecline
            // 
            btnRematchDecline.BackColor = Color.FromArgb(220, 53, 69);
            btnRematchDecline.Cursor = Cursors.Hand;
            btnRematchDecline.FlatAppearance.BorderSize = 0;
            btnRematchDecline.FlatStyle = FlatStyle.Flat;
            btnRematchDecline.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRematchDecline.ForeColor = Color.White;
            btnRematchDecline.Location = new Point(227, 690);
            btnRematchDecline.Margin = new Padding(3, 4, 3, 4);
            btnRematchDecline.Name = "btnRematchDecline";
            btnRematchDecline.Size = new Size(150, 40);
            btnRematchDecline.TabIndex = 12;
            btnRematchDecline.Text = "✗ Từ chối";
            btnRematchDecline.UseVisualStyleBackColor = false;
            btnRematchDecline.Visible = false;
            btnRematchDecline.Click += btnRematchDecline_Click;
            // 
            // lblRematchRequest
            // 
            lblRematchRequest.AutoSize = true;
            lblRematchRequest.BackColor = Color.FromArgb(255, 243, 204);
            lblRematchRequest.BorderStyle = BorderStyle.FixedSingle;
            lblRematchRequest.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRematchRequest.ForeColor = Color.FromArgb(180, 138, 14);
            lblRematchRequest.Location = new Point(18, 670);
            lblRematchRequest.Name = "lblRematchRequest";
            lblRematchRequest.Padding = new Padding(10);
            lblRematchRequest.Size = new Size(400, 40);
            lblRematchRequest.TabIndex = 13;
            lblRematchRequest.Text = "⚠ Đối thủ yêu cầu đấu lại. Bạn có đồng ý?";
            lblRematchRequest.TextAlign = ContentAlignment.MiddleCenter;
            lblRematchRequest.Visible = false;
            // 
            // btnSendChat
            // 
            btnSendChat.BackColor = Color.FromArgb(26, 127, 216);
            btnSendChat.Cursor = Cursors.Hand;
            btnSendChat.FlatAppearance.BorderSize = 0;
            btnSendChat.FlatStyle = FlatStyle.Flat;
            btnSendChat.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSendChat.ForeColor = Color.White;
            btnSendChat.Location = new Point(294, 793);
            btnSendChat.Margin = new Padding(3, 4, 3, 4);
            btnSendChat.Name = "btnSendChat";
            btnSendChat.Size = new Size(110, 33);
            btnSendChat.TabIndex = 7;
            btnSendChat.Text = "✔ Gửi";
            btnSendChat.UseVisualStyleBackColor = false;
            btnSendChat.Click += btnSendChat_Click;
            // 
            // txtChatInput
            // 
            txtChatInput.Font = new Font("Segoe UI", 10F);
            txtChatInput.Location = new Point(18, 793);
            txtChatInput.Margin = new Padding(3, 4, 3, 4);
            txtChatInput.Name = "txtChatInput";
            txtChatInput.PlaceholderText = "Nhập tin nhắn...";
            txtChatInput.Size = new Size(251, 30);
            txtChatInput.TabIndex = 6;
            txtChatInput.KeyDown += txtChatInput_KeyDown;
            // 
            // rtbChat
            // 
            rtbChat.BackColor = Color.FromArgb(248, 249, 250);
            rtbChat.BorderStyle = BorderStyle.FixedSingle;
            rtbChat.Font = new Font("Segoe UI", 9F);
            rtbChat.Location = new Point(18, 253);
            rtbChat.Margin = new Padding(3, 4, 3, 4);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.Size = new Size(386, 525);
            rtbChat.TabIndex = 5;
            rtbChat.Text = "";
            // 
            // lblChat
            // 
            lblChat.AutoSize = true;
            lblChat.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblChat.ForeColor = Color.FromArgb(33, 37, 41);
            lblChat.Location = new Point(18, 221);
            lblChat.Name = "lblChat";
            lblChat.Size = new Size(89, 28);
            lblChat.TabIndex = 4;
            lblChat.Text = "💬 Chat";
            // 
            // lblThinkingTime
            // 
            lblThinkingTime.AutoSize = true;
            lblThinkingTime.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblThinkingTime.ForeColor = Color.FromArgb(211, 55, 75);
            lblThinkingTime.Location = new Point(263, 29);
            lblThinkingTime.Name = "lblThinkingTime";
            lblThinkingTime.Size = new Size(94, 32);
            lblThinkingTime.TabIndex = 8;
            lblThinkingTime.Text = "⏱️ 30s";
            lblThinkingTime.Visible = false;
            // 
            // lblOpponentDisplayName
            // 
            lblOpponentDisplayName.AutoSize = true;
            lblOpponentDisplayName.Font = new Font("Segoe UI", 10F);
            lblOpponentDisplayName.ForeColor = Color.FromArgb(108, 117, 125);
            lblOpponentDisplayName.Location = new Point(115, 143);
            lblOpponentDisplayName.Name = "lblOpponentDisplayName";
            lblOpponentDisplayName.Size = new Size(139, 23);
            lblOpponentDisplayName.TabIndex = 3;
            lblOpponentDisplayName.Text = "Hiển thị: Player 2";
            // 
            // lblOpponentUsername
            // 
            lblOpponentUsername.AutoSize = true;
            lblOpponentUsername.Font = new Font("Segoe UI", 10F);
            lblOpponentUsername.ForeColor = Color.FromArgb(108, 117, 125);
            lblOpponentUsername.Location = new Point(115, 101);
            lblOpponentUsername.Name = "lblOpponentUsername";
            lblOpponentUsername.Size = new Size(115, 23);
            lblOpponentUsername.TabIndex = 2;
            lblOpponentUsername.Text = "Username: p2";
            lblOpponentUsername.Click += lblOpponentUsername_Click;
            // 
            // lblOpponentTitle
            // 
            lblOpponentTitle.AutoSize = true;
            lblOpponentTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblOpponentTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblOpponentTitle.Location = new Point(18, 29);
            lblOpponentTitle.Name = "lblOpponentTitle";
            lblOpponentTitle.Size = new Size(240, 30);
            lblOpponentTitle.TabIndex = 1;
            lblOpponentTitle.Text = "🎉 Thông Tin Đối Thủ";
            // 
            // picOpponentAvatar
            // 
            picOpponentAvatar.BackColor = Color.White;
            picOpponentAvatar.Location = new Point(18, 80);
            picOpponentAvatar.Margin = new Padding(3, 4, 3, 4);
            picOpponentAvatar.Name = "picOpponentAvatar";
            picOpponentAvatar.Size = new Size(91, 107);
            picOpponentAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picOpponentAvatar.TabIndex = 0;
            picOpponentAvatar.TabStop = false;
            // 
            // timerThinking
            // 
            timerThinking.Interval = 1000;
            timerThinking.Tick += timerThinking_Tick;
            // 
            // FormGame
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1257, 1013);
            Controls.Add(pnlRight);
            Controls.Add(lblStatus);
            Controls.Add(pnlChessBoard);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(912, 784);
            Name = "FormGame";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UDM_17 - Caro Client";
            Load += FormGame_Load;
            pnlRight.ResumeLayout(false);
            pnlRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picOpponentAvatar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Panel pnlChessBoard;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.PictureBox picOpponentAvatar;
        private System.Windows.Forms.Label lblOpponentTitle;
        private System.Windows.Forms.Label lblOpponentUsername;
        private System.Windows.Forms.Label lblOpponentDisplayName;
        private System.Windows.Forms.Label lblChat;
        private System.Windows.Forms.Label lblThinkingTime;
        private System.Windows.Forms.Label lblRematchRequest;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.TextBox txtChatInput;
        private System.Windows.Forms.Button btnLeaveRoom;
        private System.Windows.Forms.Button btnRematch;
        private System.Windows.Forms.Button btnRematchAccept;
        private System.Windows.Forms.Button btnRematchDecline;
        private System.Windows.Forms.Button btnSendChat;
        private System.Windows.Forms.Timer timerThinking;
    }
}