using System;
using System.Drawing;
using System.Windows.Forms;
using UDM_17.Core;

namespace UDM_17.Client
{
    public partial class FormGame : Form
    {
        private const int BOARD_SIZE = 15;
        private const int THINKING_TIME_SECONDS = 30;
        private readonly Button[,] board = new Button[BOARD_SIZE, BOARD_SIZE];
        private int _roomId;
        public int RoomId => _roomId;
        private bool _myTurn;
        private string _mySymbol = "X";
        private string _myUsername = "player";
        private int _thinkingTimeRemaining = THINKING_TIME_SECONDS;
        public event Action<string>? ChatSent;
        public event Action<MovePayload>? MoveSent;
        public event Action? LeaveRequested;

        public FormGame()
        {
            InitializeComponent();
            SetupBoardPanel();
            CreateBoard();
            SetOpponentInfo("player2", "Người chơi 2");
            AddChatMessage("Hệ thống", "Bạn đã vào trận. Hãy chờ đối thủ sẵn sàng.", true);

            // Legacy rematch controls are hidden because match restart is now automatic.
            btnRematch.Visible = false;
            btnRematchAccept.Visible = false;
            btnRematchDecline.Visible = false;
            lblRematchRequest.Visible = false;
        }

        private void SetupBoardPanel()
        {
            pnlChessBoard.Controls.Clear();
            pnlChessBoard.Padding = new Padding(0);
        }

        private void CreateBoard()
        {
            int size = Math.Min(pnlChessBoard.Width, pnlChessBoard.Height) / BOARD_SIZE;

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    Button btn = new Button();
                    btn.Width = size;
                    btn.Height = size;
                    btn.Left = j * size;
                    btn.Top = i * size;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Color.Gainsboro;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    btn.Tag = new Point(i, j);
                    btn.Click += Btn_Click;

                    board[i, j] = btn;
                    pnlChessBoard.Controls.Add(btn);
                }
            }
        }

        private void Btn_Click(object? sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null) return;

            if (btn.Text != "") return;

            if (!_myTurn)
            {
                AddChatMessage("Hệ thống", "Chưa tới lượt của bạn.", true);
                return;
            }

            if (btn.Tag is not Point pos)
            {
                return;
            }

            MoveSent?.Invoke(new MovePayload
            {
                RoomId = _roomId,
                Row = pos.X,
                Col = pos.Y,
                Symbol = _mySymbol
            });
        }

        public void SetCurrentPlayer(string username, string displayName, int roomId, string symbol, bool yourTurn)
        {
            _myUsername = username;
            _roomId = roomId;
            _mySymbol = string.IsNullOrWhiteSpace(symbol) ? "X" : symbol.Trim().ToUpperInvariant();
            _myTurn = yourTurn;

            if (_myTurn)
            {
                lblStatus.Text = $"Phòng {_roomId} | Bạn: {displayName} ({_mySymbol}) | Đến lượt bạn";
                StartThinkingTimer();
            }
            else
            {
                lblStatus.Text = $"Phòng {_roomId} | Bạn: {displayName} ({_mySymbol}) | Chờ đối thủ";
                StopThinkingTimer();
            }

            AddChatMessage("Hệ thống", $"Phòng số: {_roomId}", true);
        }

        public void HandleServerPacket(Packet packet)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(() => HandleServerPacket(packet));
                return;
            }

            switch (packet.Cmd)
            {
                case Command.MOVE:
                    var move = packet.ReadData<MovePayload>();
                    if (move != null && move.RoomId == _roomId)
                    {
                        ApplyMove(move, packet.Sender);
                    }
                    break;

                case Command.CHAT:
                    var chat = packet.ReadData<ChatPayload>();
                    if (chat != null && chat.RoomId == _roomId)
                    {
                        AddChatMessage(packet.Sender, chat.Message);
                    }
                    break;

                case Command.GAME_END:
                    var end = packet.ReadData<GameEndPayload>();
                    if (end != null && end.RoomId == _roomId)
                    {
                        _myTurn = false;
                        EndGame(end.ResultMessage);
                        AddChatMessage("Hệ thống", end.ResultMessage, true);
                        MessageBox.Show(end.ResultMessage, "Kết thúc trận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AddChatMessage("Hệ thống", "Chờ 15 giây để bắt đầu trận mới...", true);
                    }
                    break;

                case Command.GAME_END_WAIT:
                    var waitPayload = packet.ReadData<GameEndWaitPayload>();
                    if (waitPayload != null && waitPayload.RoomId == _roomId)
                    {
                        if (waitPayload.SecondsRemaining > 0)
                        {
                            lblStatus.Text = $"Phòng {_roomId} | Chờ {waitPayload.SecondsRemaining}s để bắt đầu trận mới...";
                        }
                    }
                    break;

                case Command.OPPONENT_LEFT:
                    var opponentLeft = packet.ReadData<OpponentLeftPayload>();
                    if (opponentLeft != null && opponentLeft.RoomId == _roomId)
                    {
                        _myTurn = false;
                        StopThinkingTimer();
                        AddChatMessage("Hệ thống", opponentLeft.Message, true);
                        foreach (var btn in board)
                        {
                            btn.Enabled = false;
                        }
                        lblStatus.Text = "Phòng " + _roomId + " | Đối thủ đã rời phòng...";
                    }
                    break;

                case Command.MATCH_FOUND:
                    var matchFound = packet.ReadData<MatchFoundPayload>();
                    if (matchFound != null)
                    {
                        // New match started - reset game board
                        ResetGameBoard(matchFound);
                    }
                    break;
            }

        }

        private void ApplyMove(MovePayload move, string sender)
        {
            if (move.Row < 0 || move.Row >= BOARD_SIZE || move.Col < 0 || move.Col >= BOARD_SIZE)
            {
                return;
            }

            Button btn = board[move.Row, move.Col];
            if (!string.IsNullOrEmpty(btn.Text))
            {
                return;
            }

            btn.Text = move.Symbol;
            btn.ForeColor = move.Symbol == "X" ? Color.Red : Color.Blue;
            btn.Enabled = false;

            bool mine = string.Equals(sender, _myUsername, StringComparison.OrdinalIgnoreCase);
            _myTurn = !mine;

            if (_myTurn)
            {
                lblStatus.Text = "Đến lượt bạn";
                StartThinkingTimer();
            }
            else
            {
                lblStatus.Text = "Chờ đối thủ";
                StopThinkingTimer();
            }
        }

        private void EndGame(string message)
        {
            StopThinkingTimer();
            foreach (var btn in board)
            {
                btn.Enabled = false;
            }
            lblStatus.Text = message;
        }

        private void ResetGameBoard(MatchFoundPayload match)
        {
            _roomId = match.RoomId;
            _mySymbol = match.YourSymbol;
            _myTurn = match.YourTurn;

            // Clear the board
            foreach (var btn in board)
            {
                btn.Text = "";
                btn.Enabled = true;
                btn.ForeColor = Color.Black;
            }

            // Update opponent info
            SetOpponentInfo(match.OpponentUsername, match.OpponentDisplayName);

            // Update status
            if (_myTurn)
            {
                lblStatus.Text = $"Phòng {_roomId} | Bạn: ({_mySymbol}) | Đến lượt bạn";
                StartThinkingTimer();
            }
            else
            {
                lblStatus.Text = $"Phòng {_roomId} | Bạn: ({_mySymbol}) | Chờ đối thủ";
                StopThinkingTimer();
            }

            AddChatMessage("Hệ thống", "Trận mới đã bắt đầu!", true);
        }

        public void SetOpponentInfo(string username, string displayName, Image? avatar = null)
        {
            lblOpponentUsername.Text = $"Username: {username}";
            lblOpponentDisplayName.Text = $"Display: {displayName}";
            picOpponentAvatar.Image = avatar ?? CreateInitialAvatar(displayName);
        }

        public void AddChatMessage(string sender, string message, bool isSystem = false)
        {
            if (rtbChat.TextLength > 0)
            {
                rtbChat.AppendText(Environment.NewLine);
            }

            string prefix = isSystem ? "[SYSTEM]" : sender;
            rtbChat.SelectionColor = isSystem ? Color.DimGray : Color.FromArgb(26, 115, 232);
            rtbChat.AppendText($"{prefix}: ");
            rtbChat.SelectionColor = Color.Black;
            rtbChat.AppendText(message);
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.ScrollToCaret();
        }

        private Image CreateInitialAvatar(string displayName)
        {
            Bitmap bitmap = new Bitmap(96, 96);
            using Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.FromArgb(214, 227, 248));

            string initials = "P2";
            if (!string.IsNullOrWhiteSpace(displayName))
            {
                string[] parts = displayName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                {
                    initials = parts[0][0].ToString().ToUpperInvariant();
                }
                else
                {
                    initials = string.Concat(parts[0][0], parts[^1][0]).ToUpperInvariant();
                }
            }

            using Font font = new Font("Segoe UI", 24, FontStyle.Bold);
            SizeF textSize = g.MeasureString(initials, font);
            using SolidBrush brush = new SolidBrush(Color.FromArgb(26, 115, 232));
            g.DrawString(
                initials,
                font,
                brush,
                (bitmap.Width - textSize.Width) / 2f,
                (bitmap.Height - textSize.Height) / 2f);

            return bitmap;
        }

        private void FormGame_Load(object sender, EventArgs e)
        {
            txtChatInput.Focus();
        }

        private void btnSendChat_Click(object sender, EventArgs e)
        {
            SendChat();
        }

        private void txtChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendChat();
            }
        }

        private void SendChat()
        {
            string content = txtChatInput.Text.Trim();
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            AddChatMessage("Bạn", content);
            ChatSent?.Invoke(content);
            txtChatInput.Clear();
            txtChatInput.Focus();
        }

        private void StartThinkingTimer()
        {
            _thinkingTimeRemaining = THINKING_TIME_SECONDS;
            lblThinkingTime.Visible = true;
            lblThinkingTime.Text = $"⏱️ {_thinkingTimeRemaining}s";
            lblThinkingTime.ForeColor = Color.FromArgb(211, 55, 75); // Red
            timerThinking.Start();
        }

        private void StopThinkingTimer()
        {
            timerThinking.Stop();
            lblThinkingTime.Visible = false;
            _thinkingTimeRemaining = THINKING_TIME_SECONDS;
        }

        private void timerThinking_Tick(object? sender, EventArgs e)
        {
            _thinkingTimeRemaining--;
            lblThinkingTime.Text = $"⏱️ {_thinkingTimeRemaining}s";

            // Change color to yellow when 10 seconds remaining
            if (_thinkingTimeRemaining <= 10)
            {
                lblThinkingTime.ForeColor = Color.FromArgb(255, 193, 7); // Yellow
            }

            // Time's up
            if (_thinkingTimeRemaining <= 0)
            {
                StopThinkingTimer();
                AddChatMessage("Hệ thống", "Hết thời gian suy nghĩ. Trận đấu kết thúc.", true);
                // Optionally disable board or end game
                EndGame("Hết thời gian suy nghĩ. Trận đấu kết thúc.");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            StopThinkingTimer();
            LeaveRequested?.Invoke();
            MoveSent = null;
            ChatSent = null;
        }

        private void btnLeaveRoom_Click(object sender, EventArgs e)
        {
            LeaveRequested?.Invoke();
            this.Close();
        }

        private void lblOpponentUsername_Click(object sender, EventArgs e)
        {

        }

        private void btnRematch_Click(object sender, EventArgs e)
        {
            AddChatMessage("Hệ thống", "Chức năng đấu lại thủ công đã tắt. Trận mới sẽ tự động bắt đầu sau 15 giây nếu cả 2 còn trong phòng.", true);
        }

        private void btnRematchAccept_Click(object sender, EventArgs e)
        {
            AddChatMessage("Hệ thống", "Chức năng đấu lại thủ công đã tắt.", true);
        }

        private void btnRematchDecline_Click(object sender, EventArgs e)
        {
            AddChatMessage("Hệ thống", "Chức năng đấu lại thủ công đã tắt.", true);
        }
    }
}
