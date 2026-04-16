namespace UDM_17.Server
{
    partial class FormServer
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
            btnStartServer = new Button();
            lblServerState = new Label();
            lblTotalUsers = new Label();
            lblOnlineCount = new Label();
            grpOnline = new GroupBox();
            lstOnlineUsers = new ListBox();
            grpMatches = new GroupBox();
            dgvMatches = new DataGridView();
            grpUsers = new GroupBox();
            dgvUsers = new DataGridView();
            grpLogs = new GroupBox();
            btnResetFilter = new Button();
            btnFilterLogs = new Button();
            dtpLogTo = new DateTimePicker();
            lblLogTo = new Label();
            dtpLogFrom = new DateTimePicker();
            lblLogFrom = new Label();
            dgvLogs = new DataGridView();
            refreshTimer = new System.Windows.Forms.Timer(components);
            grpOnline.SuspendLayout();
            grpMatches.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMatches).BeginInit();
            grpUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            grpLogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).BeginInit();
            SuspendLayout();
            // 
            // btnStartServer
            // 
            btnStartServer.BackColor = Color.FromArgb(211, 55, 75);
            btnStartServer.Cursor = Cursors.Hand;
            btnStartServer.FlatAppearance.BorderSize = 0;
            btnStartServer.FlatStyle = FlatStyle.Flat;
            btnStartServer.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnStartServer.ForeColor = Color.White;
            btnStartServer.Location = new Point(16, 18);
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new Size(180, 44);
            btnStartServer.TabIndex = 0;
            btnStartServer.Text = "Start Server";
            btnStartServer.UseVisualStyleBackColor = false;
            // 
            // lblServerState
            // 
            lblServerState.AutoSize = true;
            lblServerState.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblServerState.ForeColor = Color.FromArgb(46, 204, 113);
            lblServerState.Location = new Point(220, 29);
            lblServerState.Name = "lblServerState";
            lblServerState.Size = new Size(165, 25);
            lblServerState.TabIndex = 1;
            lblServerState.Text = "Trạng thái: Đã tắt";
            // 
            // lblTotalUsers
            // 
            lblTotalUsers.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTotalUsers.BackColor = Color.FromArgb(234, 243, 255);
            lblTotalUsers.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotalUsers.ForeColor = Color.FromArgb(26, 115, 232);
            lblTotalUsers.Location = new Point(432, 19);
            lblTotalUsers.Name = "lblTotalUsers";
            lblTotalUsers.Size = new Size(376, 44);
            lblTotalUsers.TabIndex = 2;
            lblTotalUsers.Text = "Tổng tài khoản: 0";
            lblTotalUsers.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOnlineCount
            // 
            lblOnlineCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOnlineCount.BackColor = Color.FromArgb(234, 243, 255);
            lblOnlineCount.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblOnlineCount.ForeColor = Color.FromArgb(26, 115, 232);
            lblOnlineCount.Location = new Point(841, 19);
            lblOnlineCount.Name = "lblOnlineCount";
            lblOnlineCount.Size = new Size(320, 44);
            lblOnlineCount.TabIndex = 3;
            lblOnlineCount.Text = "Đang online: 0";
            lblOnlineCount.TextAlign = ContentAlignment.MiddleCenter;
            lblOnlineCount.Click += lblOnlineCount_Click;
            // 
            // grpOnline
            // 
            grpOnline.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            grpOnline.BackColor = Color.White;
            grpOnline.Controls.Add(lstOnlineUsers);
            grpOnline.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            grpOnline.Location = new Point(16, 82);
            grpOnline.Name = "grpOnline";
            grpOnline.Size = new Size(350, 200);
            grpOnline.TabIndex = 4;
            grpOnline.TabStop = false;
            grpOnline.Text = "Người chơi online";
            // 
            // lstOnlineUsers
            // 
            lstOnlineUsers.Dock = DockStyle.Fill;
            lstOnlineUsers.Font = new Font("Segoe UI", 10F);
            lstOnlineUsers.FormattingEnabled = true;
            lstOnlineUsers.Location = new Point(3, 28);
            lstOnlineUsers.Name = "lstOnlineUsers";
            lstOnlineUsers.Size = new Size(344, 169);
            lstOnlineUsers.TabIndex = 0;
            // 
            // grpMatches
            // 
            grpMatches.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpMatches.BackColor = Color.White;
            grpMatches.Controls.Add(dgvMatches);
            grpMatches.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            grpMatches.Location = new Point(391, 82);
            grpMatches.Name = "grpMatches";
            grpMatches.Size = new Size(817, 200);
            grpMatches.TabIndex = 5;
            grpMatches.TabStop = false;
            grpMatches.Text = "Lịch sử đấu";
            // 
            // dgvMatches
            // 
            dgvMatches.AllowUserToAddRows = false;
            dgvMatches.AllowUserToDeleteRows = false;
            dgvMatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMatches.BackgroundColor = Color.White;
            dgvMatches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMatches.Dock = DockStyle.Fill;
            dgvMatches.Location = new Point(3, 28);
            dgvMatches.Name = "dgvMatches";
            dgvMatches.ReadOnly = true;
            dgvMatches.RowHeadersVisible = false;
            dgvMatches.RowHeadersWidth = 51;
            dgvMatches.Size = new Size(811, 169);
            dgvMatches.TabIndex = 0;
            // 
            // grpUsers
            // 
            grpUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            grpUsers.BackColor = Color.White;
            grpUsers.Controls.Add(dgvUsers);
            grpUsers.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            grpUsers.Location = new Point(16, 298);
            grpUsers.Name = "grpUsers";
            grpUsers.Size = new Size(350, 434);
            grpUsers.TabIndex = 6;
            grpUsers.TabStop = false;
            grpUsers.Text = "Tất cả tài khoản";
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.Location = new Point(3, 28);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowHeadersWidth = 51;
            dgvUsers.Size = new Size(344, 403);
            dgvUsers.TabIndex = 0;
            // 
            // grpLogs
            // 
            grpLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpLogs.BackColor = Color.White;
            grpLogs.Controls.Add(btnResetFilter);
            grpLogs.Controls.Add(btnFilterLogs);
            grpLogs.Controls.Add(dtpLogTo);
            grpLogs.Controls.Add(lblLogTo);
            grpLogs.Controls.Add(dtpLogFrom);
            grpLogs.Controls.Add(lblLogFrom);
            grpLogs.Controls.Add(dgvLogs);
            grpLogs.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            grpLogs.Location = new Point(391, 298);
            grpLogs.Name = "grpLogs";
            grpLogs.Size = new Size(823, 440);
            grpLogs.TabIndex = 7;
            grpLogs.TabStop = false;
            grpLogs.Text = "Nhật ký server";
            // 
            // btnResetFilter
            // 
            btnResetFilter.BackColor = Color.FromArgb(108, 117, 125);
            btnResetFilter.Cursor = Cursors.Hand;
            btnResetFilter.FlatAppearance.BorderSize = 0;
            btnResetFilter.FlatStyle = FlatStyle.Flat;
            btnResetFilter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnResetFilter.ForeColor = Color.White;
            btnResetFilter.Location = new Point(517, 33);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(120, 30);
            btnResetFilter.TabIndex = 5;
            btnResetFilter.Text = "Xóa bộ lọc";
            btnResetFilter.UseVisualStyleBackColor = false;
            btnResetFilter.Click += btnResetFilter_Click;
            // 
            // btnFilterLogs
            // 
            btnFilterLogs.BackColor = Color.FromArgb(26, 127, 216);
            btnFilterLogs.Cursor = Cursors.Hand;
            btnFilterLogs.FlatAppearance.BorderSize = 0;
            btnFilterLogs.FlatStyle = FlatStyle.Flat;
            btnFilterLogs.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFilterLogs.ForeColor = Color.White;
            btnFilterLogs.Location = new Point(374, 33);
            btnFilterLogs.Name = "btnFilterLogs";
            btnFilterLogs.Size = new Size(115, 30);
            btnFilterLogs.TabIndex = 4;
            btnFilterLogs.Text = "Lọc log";
            btnFilterLogs.UseVisualStyleBackColor = false;
            btnFilterLogs.Click += btnFilterLogs_Click;
            // 
            // dtpLogTo
            // 
            dtpLogTo.Format = DateTimePickerFormat.Short;
            dtpLogTo.Location = new Point(236, 31);
            dtpLogTo.Name = "dtpLogTo";
            dtpLogTo.Size = new Size(110, 32);
            dtpLogTo.TabIndex = 3;
            // 
            // lblLogTo
            // 
            lblLogTo.AutoSize = true;
            lblLogTo.Font = new Font("Segoe UI", 9F);
            lblLogTo.Location = new Point(177, 38);
            lblLogTo.Name = "lblLogTo";
            lblLogTo.Size = new Size(39, 20);
            lblLogTo.TabIndex = 2;
            lblLogTo.Text = "Đến:";
            // 
            // dtpLogFrom
            // 
            dtpLogFrom.Format = DateTimePickerFormat.Short;
            dtpLogFrom.Location = new Point(50, 31);
            dtpLogFrom.Name = "dtpLogFrom";
            dtpLogFrom.Size = new Size(110, 32);
            dtpLogFrom.TabIndex = 1;
            // 
            // lblLogFrom
            // 
            lblLogFrom.AutoSize = true;
            lblLogFrom.Font = new Font("Segoe UI", 9F);
            lblLogFrom.Location = new Point(15, 38);
            lblLogFrom.Name = "lblLogFrom";
            lblLogFrom.Size = new Size(29, 20);
            lblLogFrom.TabIndex = 0;
            lblLogFrom.Text = "Từ:";
            // 
            // dgvLogs
            // 
            dgvLogs.AllowUserToAddRows = false;
            dgvLogs.AllowUserToDeleteRows = false;
            dgvLogs.AllowUserToResizeRows = false;
            dgvLogs.BackgroundColor = Color.White;
            dgvLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLogs.Location = new Point(3, 69);
            dgvLogs.Name = "dgvLogs";
            dgvLogs.ReadOnly = true;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.RowHeadersWidth = 51;
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.Size = new Size(814, 365);
            dgvLogs.TabIndex = 6;
            dgvLogs.CellContentClick += dgvLogs_CellContentClick;
            // 
            // refreshTimer
            // 
            refreshTimer.Interval = 1500;
            // 
            // FormServer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(1226, 750);
            Controls.Add(grpLogs);
            Controls.Add(grpUsers);
            Controls.Add(grpMatches);
            Controls.Add(grpOnline);
            Controls.Add(lblOnlineCount);
            Controls.Add(lblTotalUsers);
            Controls.Add(lblServerState);
            Controls.Add(btnStartServer);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(900, 550);
            Name = "FormServer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UDM_17 - Quản lý Server";
            Load += FormServer_Load;
            grpOnline.ResumeLayout(false);
            grpMatches.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMatches).EndInit();
            grpUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            grpLogs.ResumeLayout(false);
            grpLogs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Label lblServerState;
        private System.Windows.Forms.Label lblTotalUsers;
        private System.Windows.Forms.Label lblOnlineCount;
        private System.Windows.Forms.GroupBox grpOnline;
        private System.Windows.Forms.ListBox lstOnlineUsers;
        private System.Windows.Forms.GroupBox grpMatches;
        private System.Windows.Forms.DataGridView dgvMatches;
        private System.Windows.Forms.GroupBox grpUsers;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.GroupBox grpLogs;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.Label lblLogFrom;
        private System.Windows.Forms.DateTimePicker dtpLogFrom;
        private System.Windows.Forms.Label lblLogTo;
        private System.Windows.Forms.DateTimePicker dtpLogTo;
        private System.Windows.Forms.Button btnFilterLogs;
        private System.Windows.Forms.Button btnResetFilter;
        private System.Windows.Forms.Timer refreshTimer;
    }
}