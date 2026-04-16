using System;
using System.Drawing;
using System.Windows.Forms;
using UDM_17.Core;

namespace UDM_17.Client
{
    public partial class FormLobby : Form
    {
        private readonly SocketClient _socket = new SocketClient();
        private string _username = string.Empty;
        private string _displayName = string.Empty;
        private string _avatarBase64 = string.Empty;
        private string _userId = string.Empty;
        private int _score;
        private int _createdRoomId;
        private bool _loggedIn;
        private FormLogin? _currentLoginForm;
        private FormGame? _activeGameForm;

        public FormLobby()
        {
            InitializeComponent();

            _socket.PacketReceived += OnPacketReceived;
            _socket.Disconnected += () => BeginInvoke(() =>
            {
                lblStatus.Text = "Mat ket noi server.";
                _loggedIn = false;
                UpdateUserInfo();
                ToggleActions(false);
            });
            ToggleActions(false);
            UpdateUserInfo();
        }

        private async void FormLobby_Load(object sender, EventArgs e)
        {
            await ConnectToServerAsync();
        }

        private async Task ConnectToServerAsync()
        {
            const int maxAttempts = 5;
            const int retryDelayMs = 1000;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    lblStatus.Text = $"Dang ket noi server... ({attempt}/{maxAttempts})";
                    await _socket.ConnectAsync();
                    lblStatus.Text = "Da ket noi server 127.0.0.1:1234";
                    ShowLoginDialog();
                    return;
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Khong ket noi duoc server: {ex.Message}";
                    if (attempt == maxAttempts)
                    {
                        MessageBox.Show("Khong the ket noi server. Hay khoi dong UDM_17.Server truoc va thu lai.", "Ket noi that bai", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    await Task.Delay(retryDelayMs);
                }
            }
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            using FormRegister register = new FormRegister();
            if (register.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            AuthRequest req = new AuthRequest
            {
                Username = register.Username,
                Password = register.Password,
                DisplayName = register.DisplayNameText
            };

            await _socket.SendAsync(Packet.Create(Command.REGISTER, req, req.Username));
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            if (login.ShowDialog(this) != DialogResult.OK)
            {
                login.Dispose();
                return;
            }

            await SendLoginAsync(login.Username, login.Password, login);
        }

        private async void btnQuickMatch_Click(object sender, EventArgs e)
        {
            if (!_loggedIn)
            {
                ShowLoginDialog();
                return;
            }

            ShowWaiting("Dang tim doi thu...");
            lblStatus.Text = "Dang tim doi thu...";
            _createdRoomId = 0;
            await _socket.SendAsync(Packet.Create(Command.QUICK_MATCH, new { }, _username));
        }

        private async void btnCreateRoom_Click(object sender, EventArgs e)
        {
            if (!_loggedIn)
            {
                ShowLoginDialog();
                return;
            }

            await _socket.SendAsync(Packet.Create(Command.CREATE_ROOM, new { }, _username));
        }

        private async void btnCancelWait_Click(object sender, EventArgs e)
        {
            HideWaiting();
            lblStatus.Text = "Đã hủy chờ.";
            _createdRoomId = 0;
            await _socket.SendAsync(Packet.Create(Command.LEAVE_ROOM, new { }, _username));
            await _socket.SendAsync(Packet.Create(Command.ROOM_LIST, new { }, _username));
        }

        private async void btnJoinRoom_Click(object sender, EventArgs e)
        {
            if (!_loggedIn)
            {
                ShowLoginDialog();
                return;
            }

            if (!int.TryParse(txtRoomId.Text.Trim(), out int roomId) || roomId <= 0)
            {
                MessageBox.Show("ID phòng không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ShowWaiting($"Đang vào phòng {roomId}...");
            await _socket.SendAsync(Packet.Create(Command.JOIN_ROOM, new RoomJoinPayload { RoomId = roomId }, _username));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            HideWaiting();
            _loggedIn = false;
            _username = string.Empty;
            _displayName = string.Empty;
            _avatarBase64 = string.Empty;
            _score = 0;
            _userId = string.Empty;
            ToggleActions(false);
            UpdateUserInfo();
            lblStatus.Text = "Đã đăng xuất. Vui lòng đăng nhập lại để chơi.";
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            using FormRanking ranking = new FormRanking();
            ranking.LoadRows(BuildSampleRankingRows());
            ranking.ShowDialog(this);
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            if (!_loggedIn)
            {
                ShowLoginDialog();
                return;
            }

            using FormProfile profile = new FormProfile();
            profile.LoadProfile(_userId, _username, _displayName, _score, TryDecodeBase64Image(_avatarBase64));
            profile.ShowDialog(this);
        }

        private void lstOpenRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstOpenRooms.SelectedItem is RoomListDisplay item)
            {
                txtRoomId.Text = item.RoomId.ToString();
            }
        }

        private void OnPacketReceived(Packet packet)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => OnPacketReceived(packet));
                return;
            }

            switch (packet.Cmd)
            {
                case Command.AUTH_OK:
                    HandleAuthOk(packet);
                    break;

                case Command.AUTH_FAIL:
                    HideWaiting();
                    HandleAuthFail(packet);
                    break;

                case Command.ROOM_CREATED:
                    HandleRoomCreated(packet);
                    break;

                case Command.ROOM_LIST:
                    HandleRoomList(packet);
                    break;

                case Command.MATCH_FOUND:
                    HideWaiting();
                    _createdRoomId = 0;
                    HandleMatchFound(packet);
                    break;

                case Command.ERROR:
                    HideWaiting();
                    var err = packet.ReadData<dynamic>();
                    lblStatus.Text = err?.Message?.ToString() ?? "Loi khong xac dinh.";
                    break;

                case Command.OPPONENT_LEFT:
                    HandleOpponentLeft(packet);
                    break;
            }
        }

        private void HandleOpponentLeft(Packet packet)
        {
            var payload = packet.ReadData<OpponentLeftPayload>();
            if (payload != null)
            {
                MessageBox.Show(payload.Message, "Đối thủ rời phòng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void HandleAuthOk(Packet packet)
        {
            AuthResponse? auth = packet.ReadData<AuthResponse>();
            if (auth != null && auth.Username.Length > 0)
            {
                _loggedIn = true;
                _userId = auth.UserId;
                _username = auth.Username;
                _displayName = auth.DisplayName;
                _avatarBase64 = auth.AvatarBase64;
                _score = auth.Score;
                ToggleActions(true);
                UpdateUserInfo();
                lblStatus.Text = auth.Message.Length > 0 ? auth.Message : $"Xin chào {_displayName}";
                _ = _socket.SendAsync(Packet.Create(Command.ROOM_LIST, new { }, _username));

                if (_currentLoginForm != null && !_currentLoginForm.IsDisposed)
                {
                    _currentLoginForm.DialogResult = DialogResult.Cancel;
                    _currentLoginForm.Close();
                    _currentLoginForm.Dispose();
                }
                _currentLoginForm = null;
                return;
            }

            var msg = packet.ReadData<dynamic>();
            lblStatus.Text = msg?.Message?.ToString() ?? "Thanh cong";
        }

        private void HandleAuthFail(Packet packet)
        {
            var msg = packet.ReadData<dynamic>();
            string errorMsg = msg?.Message?.ToString() ?? "That bai";
            lblStatus.Text = errorMsg;
            _loggedIn = false;
            UpdateUserInfo();
            ToggleActions(false);
            MessageBox.Show(errorMsg, "Dang nhap that bai", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void HandleRoomCreated(Packet packet)
        {
            RoomInfo? info = packet.ReadData<RoomInfo>();
            if (info == null)
            {
                return;
            }

            _createdRoomId = info.RoomId;
            ShowWaiting($"ID phòng: {info.RoomId}");
            lblStatus.Text = $"Đã tạo phòng {info.RoomId}, đang chờ người chơi...";
        }

        private void HandleRoomList(Packet packet)
        {
            RoomListPayload? payload = packet.ReadData<RoomListPayload>();
            List<RoomInfo> rooms = payload?.Rooms ?? new List<RoomInfo>();

            lstOpenRooms.Items.Clear();
            foreach (var room in rooms)
            {
                lstOpenRooms.Items.Add(new RoomListDisplay(room.RoomId, room.HostDisplayName, room.HostUsername));
            }

            lblRooms.Text = $"Phòng hiện có ({rooms.Count})";
            lblRoomHint.Visible = rooms.Count == 0;
            if (rooms.Count == 0)
            {
                lblRoomHint.Text = "Chưa có phòng nào đang mở";
            }
            else
            {
                lblRoomHint.Text = "Chọn phòng ở danh sách bên dưới";
            }

            if (_createdRoomId > 0 && rooms.All(r => r.RoomId != _createdRoomId))
            {
                _createdRoomId = 0;
            }
        }

        private void HandleMatchFound(Packet packet)
        {
            if (!_loggedIn)
            {
                return;
            }

            if (_activeGameForm != null && !_activeGameForm.IsDisposed)
            {
                // Active game form already listens to socket packets and will reset board itself.
                return;
            }

            MatchFoundPayload? payload = packet.ReadData<MatchFoundPayload>();
            if (payload == null)
            {
                return;
            }

            FormGame game = new FormGame();
            _activeGameForm = game;
            Image? avatar = TryDecodeBase64Image(payload.OpponentAvatarBase64);
            game.SetOpponentInfo(payload.OpponentUsername, payload.OpponentDisplayName, avatar);
            game.SetCurrentPlayer(_username, _displayName, payload.RoomId, payload.YourSymbol, payload.YourTurn);

            game.ChatSent += async message =>
            {
                ChatPayload chat = new ChatPayload { RoomId = game.RoomId, Message = message };
                await _socket.SendAsync(Packet.Create(Command.CHAT, chat, _username));
            };

            game.MoveSent += async move =>
            {
                move.RoomId = game.RoomId;
                await _socket.SendAsync(Packet.Create(Command.MOVE, move, _username));
            };

            game.LeaveRequested += async () =>
            {
                await _socket.SendAsync(Packet.Create(Command.LEAVE_ROOM, new { }, _username));
            };

            _socket.PacketReceived += game.HandleServerPacket;
            game.FormClosed += (s, e) =>
            {
                _socket.PacketReceived -= game.HandleServerPacket;
                _activeGameForm = null;
            };

            game.Show();
            this.Hide();
            game.FormClosed += async (s, e) =>
            {
                this.Show();
                this.BringToFront();
                HideWaiting();
                if (_loggedIn)
                {
                    await _socket.SendAsync(Packet.Create(Command.ROOM_LIST, new { }, _username));
                }
            };
        }

        private void ShowLoginDialog()
        {
            if (_loggedIn)
            {
                return;
            }

            FormLogin login = new FormLogin();
            var dialog = login.ShowDialog(this);
            if (dialog != DialogResult.OK)
            {
                login.Dispose();
                return;
            }

            _ = SendLoginAsync(login.Username, login.Password, login);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (_loggedIn)
            {
                _ = _socket.SendAsync(Packet.Create(Command.ROOM_LIST, new { }, _username));
            }
        }

        private async Task SendLoginAsync(string username, string password, FormLogin? loginForm = null)
        {
            _username = username;
            AuthRequest req = new AuthRequest
            {
                Username = username,
                Password = password
            };

            _currentLoginForm = loginForm;
            await _socket.SendAsync(Packet.Create(Command.LOGIN, req, username));
        }

        private void ToggleActions(bool enabled)
        {
            btnQuickMatch.Enabled = enabled;
            btnCreateRoom.Enabled = enabled;
            btnJoinRoom.Enabled = enabled;
            btnProfile.Enabled = enabled;
            btnRanking.Enabled = enabled;
            btnLogin.Visible = !enabled;
            btnRegister.Visible = !enabled;
            btnLogout.Visible = enabled;
        }

        private void UpdateUserInfo()
        {
            if (_loggedIn)
            {
                lblUserInfo.Text = $"Xin chào, {_displayName} • Điểm: {_score}";
                lblUserWelcome.Text = _displayName;
                lblUserWelcome.Visible = true;
                picUserAvatar.Image = TryDecodeBase64Image(_avatarBase64) ?? CreateInitialAvatar(_displayName);
                picUserAvatar.Visible = true;
            }
            else
            {
                lblUserInfo.Text = "Đăng nhập hoặc đăng ký để bắt đầu chơi Caro.";
                lblUserWelcome.Visible = false;
                picUserAvatar.Visible = false;
            }
        }

        private Image CreateInitialAvatar(string displayName)
        {
            string initial = string.IsNullOrWhiteSpace(displayName)
                ? "?"
                : displayName.Trim()[0].ToString().ToUpper();

            var avatar = new Bitmap(64, 64);
            using (Graphics g = Graphics.FromImage(avatar))
            {
                g.Clear(System.Drawing.Color.FromArgb(26, 127, 216));
                using var font = new Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Pixel);
                using var brush = new SolidBrush(System.Drawing.Color.White);
                var layout = new RectangleF(0, 0, 64, 64);
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(initial, font, brush, layout, format);
            }

            return avatar;
        }

        private void ShowWaiting(string title)
        {
            lblWaitingTitle.Text = title;
            pnlWaiting.BringToFront();
            pnlWaiting.Visible = true;
        }

        private void HideWaiting()
        {
            pnlWaiting.Visible = false;
        }

        private List<RankingRow> BuildSampleRankingRows()
        {
            var rows = new List<RankingRow>();

            if (_loggedIn)
            {
                rows.Add(new RankingRow(1, string.IsNullOrEmpty(_userId) ? "-" : _userId, _username, _displayName, _score));
            }

            return rows;
        }

        private static Image? TryDecodeBase64Image(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
            {
                return null;
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                using MemoryStream ms = new MemoryStream(bytes);
                return Image.FromStream(ms);
            }
            catch
            {
                return null;
            }
        }

        private void lblUserWelcome_Click(object sender, EventArgs e)
        {

        }
    }

    public record RankingRow(int Rank, string UserId, string Username, string DisplayName, int Score);

    internal class RoomListDisplay
    {
        public int RoomId { get; }
        public string HostDisplayName { get; }
        public string HostUsername { get; }

        public RoomListDisplay(int roomId, string hostDisplayName, string hostUsername)
        {
            RoomId = roomId;
            HostDisplayName = hostDisplayName;
            HostUsername = hostUsername;
        }

        public override string ToString() => $"{RoomId} - {HostDisplayName} ({HostUsername})";
    }
}
