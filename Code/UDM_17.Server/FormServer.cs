namespace UDM_17.Server
{
    public partial class FormServer : Form
    {
        private readonly DatabaseManager _database;
        private readonly ServerManager _server;
        private bool _started;
        private bool _logFilterApplied;

        public FormServer()
        {
            InitializeComponent();
            this.Text = "UDM_17 - Caro Server Manager";
            string dbPath = Path.Combine(AppContext.BaseDirectory, "udm17.db");
            _database = new DatabaseManager(dbPath);
            _server = new ServerManager(_database);
            _server.OnLog += AppendLog;
            btnStartServer.Click += (s, e) => StartServer();
            refreshTimer.Tick += (s, e) => RefreshDashboard();
        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            SetupGrids();
            dtpLogFrom.Value = DateTime.Today.AddDays(-7);
            dtpLogTo.Value = DateTime.Today;
            RefreshDashboard();
            StartServer();
        }

        private void SetupGrids()
        {
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.Columns.Clear();
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Username", DataPropertyName = "Username" });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ten hien thi", DataPropertyName = "DisplayName" });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Score", DataPropertyName = "Score" });

            dgvMatches.AutoGenerateColumns = false;
            dgvMatches.Columns.Clear();
            dgvMatches.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Room", DataPropertyName = "RoomId", Width = 80 });
            dgvMatches.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thoi gian", DataPropertyName = "StartedAt", Width = 140 });
            dgvMatches.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Player X", DataPropertyName = "PlayerX", Width = 100 });
            dgvMatches.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Player O", DataPropertyName = "PlayerO", Width = 100 });
            dgvMatches.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Winner", DataPropertyName = "Winner", Width = 100 });

            dgvLogs.AutoGenerateColumns = false;
            dgvLogs.Columns.Clear();
            dgvLogs.AllowUserToResizeColumns = true;
            dgvLogs.AllowUserToResizeRows = false;

            var colTime = new DataGridViewTextBoxColumn { HeaderText = "Thoi gian", DataPropertyName = "CreatedAt", Width = 140 };
            var colLevel = new DataGridViewTextBoxColumn { HeaderText = "Cap do", DataPropertyName = "Level", Width = 70 };
            var colMessage = new DataGridViewTextBoxColumn { HeaderText = "Thong diep", DataPropertyName = "Message" };

            dgvLogs.Columns.Add(colTime);
            dgvLogs.Columns.Add(colLevel);
            dgvLogs.Columns.Add(colMessage);

            colMessage.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void StartServer()
        {
            if (_started)
            {
                return;
            }

            try
            {
                _server.Start();
                _started = true;
                btnStartServer.Enabled = false;
                btnStartServer.Text = "Server dang chay";
                lblServerState.Text = "Trang thai: Dang chay";
                AppendLog("[SYSTEM] Server dang lang nghe tai cong 1234");
                refreshTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Loi khoi dong server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => AppendLog(message));
                return;
            }

            RefreshLogs();
        }

        private void RefreshDashboard()
        {
            if (IsDisposed)
            {
                return;
            }

            lblTotalUsers.Text = $"Tong tai khoan: {_database.GetTotalUsers()}";
            lblOnlineCount.Text = $"Online now: {_server.GetOnlineCount()}";

            var users = _database.GetUsers();
            dgvUsers.DataSource = users;

            var matches = _database.GetRecentMatches(50);
            dgvMatches.DataSource = matches;

            RefreshLogs();

            lstOnlineUsers.Items.Clear();
            foreach (var user in _server.GetOnlineUsers())
            {
                lstOnlineUsers.Items.Add(user);
            }
        }

        private void RefreshLogs()
        {
            if (IsDisposed)
            {
                return;
            }

            DateTime? from = null;
            DateTime? to = null;
            if (_logFilterApplied)
            {
                from = dtpLogFrom.Value.Date;
                to = dtpLogTo.Value.Date.AddDays(1).AddSeconds(-1);
            }

            var logs = _database.GetServerLogs(from, to, 500);
            dgvLogs.DataSource = logs;
            if (dgvLogs.Rows.Count > 0)
            {
                dgvLogs.FirstDisplayedScrollingRowIndex = 0;
            }
        }

        private void ApplyLogFilter()
        {
            _logFilterApplied = true;
            RefreshLogs();
        }

        private void ResetLogFilter()
        {
            _logFilterApplied = false;
            dtpLogFrom.Value = DateTime.Today.AddDays(-7);
            dtpLogTo.Value = DateTime.Today;
            RefreshLogs();
        }

        private void btnFilterLogs_Click(object sender, EventArgs e)
        {
            ApplyLogFilter();
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            ResetLogFilter();
        }

        private void dgvLogs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblOnlineCount_Click(object sender, EventArgs e)
        {

        }
    }
}