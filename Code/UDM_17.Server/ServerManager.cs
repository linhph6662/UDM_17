using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using UDM_17.Core;

namespace UDM_17.Server
{
    public class ServerManager
    {
        private readonly DatabaseManager _db;
        private const int DefaultPort = 1234;
        private TcpListener _listener;
        private readonly List<ClientSession> _sessions = new List<ClientSession>();
        private readonly Queue<ClientSession> _quickMatchQueue = new Queue<ClientSession>();
        private readonly Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
        private readonly Dictionary<int, PendingRoom> _pendingRooms = new Dictionary<int, PendingRoom>();
        private readonly Random _random = new Random();
        private readonly object _lockObj = new object();
        public Action<string> OnLog;

        public ServerManager(DatabaseManager db)
        {
            _db = db;
        }

        private int GenerateRoomId()
        {
            // Tạo ID phòng 6-8 số
            return _random.Next(100000, 99999999);
        }

        public int GetOnlineCount()
        {
            lock (_lockObj)
            {
                return _sessions.Count(s => !string.IsNullOrWhiteSpace(s.Username));
            }
        }

        public List<string> GetOnlineUsers()
        {
            lock (_lockObj)
            {
                return _sessions
                    .Where(s => !string.IsNullOrWhiteSpace(s.Username))
                    .Select(s => $"{s.DisplayName} ({s.Username})")
                    .Distinct()
                    .OrderBy(s => s)
                    .ToList();
            }
        }

        public void Start()
        {
            int port = DefaultPort;
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            Log("INFO", $"Server da bat dau tai cong {port}");

            Task.Run(() =>
            {
                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    var session = new ClientSession(client);
                    lock (_lockObj)
                    {
                        _sessions.Add(session);
                    }

                    Log("INFO", $"Client ket noi: {client.Client.RemoteEndPoint}");
                    Task.Run(() => Receive(session));
                }
            });

            // Start rematch wait timer
            Task.Run(() => RematchWaitTimer());
        }

        private void RematchWaitTimer()
        {
            while (true)
            {
                try
                {
                    System.Threading.Thread.Sleep(1000); // Check every 1 second

                    lock (_lockObj)
                    {
                        var waitingRooms = _rooms.Values.Where(r => r.IsWaitingForRematch).ToList();
                        foreach (var room in waitingRooms)
                        {
                            int elapsed = (int)(DateTime.Now - room.RematchWaitStartTime).TotalSeconds;
                            int remaining = GameRoom.REMATCH_WAIT_SECONDS - elapsed;

                            // Send countdown updates
                            if (remaining > 0)
                            {
                                var waitPayload = new GameEndWaitPayload
                                {
                                    RoomId = room.RoomId,
                                    SecondsRemaining = remaining
                                };
                                Send(room.PlayerX, Packet.Create(Command.GAME_END_WAIT, waitPayload, "server"));
                                Send(room.PlayerO, Packet.Create(Command.GAME_END_WAIT, waitPayload, "server"));
                            }

                            // Time's up - check if both players are still here
                            if (elapsed >= GameRoom.REMATCH_WAIT_SECONDS)
                            {
                                // Both players still in room - restart in the same room id.
                                GameRoom newRoom = new GameRoom(room.RoomId, room.PlayerX, room.PlayerO);
                                _rooms[room.RoomId] = newRoom;

                                MatchFoundPayload p1Payload = new MatchFoundPayload
                                {
                                    RoomId = room.RoomId,
                                    OpponentUsername = room.PlayerO.Username,
                                    OpponentDisplayName = room.PlayerO.DisplayName,
                                    OpponentAvatarBase64 = room.PlayerO.AvatarBase64,
                                    YourSymbol = "X",
                                    YourTurn = true
                                };

                                MatchFoundPayload p2Payload = new MatchFoundPayload
                                {
                                    RoomId = room.RoomId,
                                    OpponentUsername = room.PlayerX.Username,
                                    OpponentDisplayName = room.PlayerX.DisplayName,
                                    OpponentAvatarBase64 = room.PlayerX.AvatarBase64,
                                    YourSymbol = "O",
                                    YourTurn = false
                                };

                                Send(room.PlayerX, Packet.Create(Command.MATCH_FOUND, p1Payload, "server"));
                                Send(room.PlayerO, Packet.Create(Command.MATCH_FOUND, p2Payload, "server"));

                                Log("INFO", $"Tro choi moi da bat dau lai trong phong {room.RoomId}: {room.PlayerX.Username}(X) vs {room.PlayerO.Username}(O)");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log("WARN", $"Loi trong RematchWaitTimer: {ex.Message}");
                }
            }
        }

        private void Receive(ClientSession session)
        {
            try
            {
                while (true)
                {
                    string json = session.Reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        break;
                    }

                    Packet packet = Packet.FromJson(json);
                    if (packet == null)
                    {
                        continue;
                    }

                    HandlePacket(session, packet);
                }
            }
            catch (Exception ex)
            {
                Log("WARN", $"Loi nhan du lieu: {ex.Message}");
            }
            finally
            {
                Disconnect(session);
            }
        }

        private void HandlePacket(ClientSession session, Packet packet)
        {
            switch (packet.Cmd)
            {
                case Command.REGISTER:
                    HandleRegister(session, packet);
                    break;

                case Command.LOGIN:
                    HandleLogin(session, packet);
                    break;

                case Command.QUICK_MATCH:
                    HandleQuickMatch(session);
                    break;

                case Command.CREATE_ROOM:
                    HandleCreateRoom(session);
                    break;

                case Command.JOIN_ROOM:
                    HandleJoinRoom(session, packet);
                    break;

                case Command.MOVE:
                    HandleMove(session, packet);
                    break;

                case Command.CHAT:
                    HandleChat(session, packet);
                    break;

                case Command.LEAVE_ROOM:
                    HandleLeaveRoom(session);
                    break;

                case Command.TURN_TIMEOUT:
                    HandleTurnTimeout(session, packet);
                    break;

                case Command.GET_RANKING:
                    HandleGetRanking(session, packet);
                    break;

                case Command.ROOM_LIST:
                    SendRoomListTo(session);
                    break;
            }
        }

        private void HandleRegister(ClientSession session, Packet packet)
        {
            AuthRequest req = packet.ReadData<AuthRequest>();
            if (req == null)
            {
                Send(session, Packet.Create(Command.AUTH_FAIL, new { Message = "Du lieu register khong hop le." }, "server"));
                return;
            }

            var result = _db.Register(req.Username, req.Password, req.DisplayName);
            if (!result.Success)
            {
                Send(session, Packet.Create(Command.AUTH_FAIL, new { Message = result.Message }, "server"));
                Log("WARN", $"Register that bai: {req.Username} - {result.Message}");
                return;
            }

            var user = _db.GetUserByUsername(req.Username);
            if (user == null)
            {
                Send(session, Packet.Create(Command.AUTH_FAIL, new { Message = "Dang ky thanh cong nhung khong the dang nhap tu dong." }, "server"));
                Log("WARN", $"Register thanh cong nhung lay thong tin user that bai: {req.Username}");
                return;
            }

            session.Username = user.Username;
            session.DisplayName = user.DisplayName;
            session.AvatarBase64 = user.AvatarBase64;

            var response = new AuthResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                DisplayName = user.DisplayName,
                Score = user.Score,
                AvatarBase64 = user.AvatarBase64,
                Message = "Đăng ký thành công và đăng nhập tự động." 
            };

            Send(session, Packet.Create(Command.AUTH_OK, response, "server"));
            Log("INFO", $"Register va login thanh cong: {req.Username}");
        }

        private void HandleLogin(ClientSession session, Packet packet)
        {
            AuthRequest req = packet.ReadData<AuthRequest>();
            if (req == null)
            {
                Send(session, Packet.Create(Command.AUTH_FAIL, new { Message = "Du lieu login khong hop le." }, "server"));
                return;
            }

            UserRecord user = _db.Login(req.Username, req.Password);
            if (user == null)
            {
                Send(session, Packet.Create(Command.AUTH_FAIL, new { Message = "Sai tai khoan hoac mat khau." }, "server"));
                Log("WARN", $"Login that bai: {req.Username}");
                return;
            }

            session.Username = user.Username;
            session.DisplayName = user.DisplayName;
            session.AvatarBase64 = user.AvatarBase64;

            var response = new AuthResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                DisplayName = user.DisplayName,
                Score = user.Score,
                AvatarBase64 = user.AvatarBase64,
                Message = "Dang nhap thanh cong."
            };

            Send(session, Packet.Create(Command.AUTH_OK, response, "server"));
            Log("INFO", $"Login thanh cong: {user.Username}");
        }

        private void HandleQuickMatch(ClientSession session)
        {
            if (string.IsNullOrEmpty(session.Username))
            {
                Send(session, Packet.Create(Command.ERROR, new { Message = "Can dang nhap truoc khi quick-match." }, "server"));
                return;
            }

            lock (_lockObj)
            {
                if (session.RoomId > 0)
                {
                    return;
                }

                RemoveFromQueue(session);
                RemovePendingByHost(session);

                PendingRoom? waitingRoom = _pendingRooms.Values
                    .Where(r => r.Host != session && r.Host.RoomId <= 0 && !string.IsNullOrWhiteSpace(r.Host.Username))
                    .OrderBy(r => r.CreatedAt)
                    .FirstOrDefault();

                if (waitingRoom != null)
                {
                    _pendingRooms.Remove(waitingRoom.RoomId);
                    RemoveFromQueue(waitingRoom.Host);
                    StartMatchInternal(waitingRoom.Host, session, waitingRoom.RoomId);
                    Log("INFO", $"{session.Username} quick-match vao phong cho {waitingRoom.RoomId} cua {waitingRoom.Host.Username}");
                    BroadcastRoomList();
                    return;
                }

                if (!_quickMatchQueue.Contains(session))
                {
                    _quickMatchQueue.Enqueue(session);
                }

                if (_quickMatchQueue.Count < 2)
                {
                    Log("INFO", $"{session.Username} dang cho quick-match.");
                    return;
                }

                ClientSession p1 = _quickMatchQueue.Dequeue();
                ClientSession p2 = _quickMatchQueue.Dequeue();
                StartMatchInternal(p1, p2);
            }
        }

        private void HandleCreateRoom(ClientSession session)
        {
            if (string.IsNullOrEmpty(session.Username))
            {
                Send(session, Packet.Create(Command.ERROR, new { Message = "Can dang nhap truoc khi tao phong." }, "server"));
                return;
            }

            lock (_lockObj)
            {
                if (session.RoomId > 0)
                {
                    Send(session, Packet.Create(Command.ERROR, new { Message = "Ban dang trong mot phong khac." }, "server"));
                    return;
                }

                RemoveFromQueue(session);
                RemovePendingByHost(session);

                ClientSession? quickWaiting = _quickMatchQueue
                    .FirstOrDefault(s => s != session && s.RoomId <= 0 && !string.IsNullOrWhiteSpace(s.Username));

                if (quickWaiting != null)
                {
                    RemoveFromQueue(quickWaiting);
                    RemovePendingByHost(quickWaiting);

                    int instantRoomId = GenerateRoomId();
                    StartMatchInternal(session, quickWaiting, instantRoomId);
                    Log("INFO", $"{session.Username} tao phong va duoc ghep ngay voi {quickWaiting.Username} dang quick-match");
                    BroadcastRoomList();
                    return;
                }

                int roomId = GenerateRoomId();
                _pendingRooms[roomId] = new PendingRoom
                {
                    RoomId = roomId,
                    Host = session,
                    CreatedAt = DateTime.Now
                };

                var roomInfo = new RoomInfo
                {
                    RoomId = roomId,
                    HostUsername = session.Username,
                    HostDisplayName = session.DisplayName,
                    Status = "Waiting"
                };

                Send(session, Packet.Create(Command.ROOM_CREATED, roomInfo, "server"));
                Log("INFO", $"{session.Username} da tao phong {roomId}");
                BroadcastRoomList();
            }
        }

        private void HandleJoinRoom(ClientSession session, Packet packet)
        {
            if (string.IsNullOrEmpty(session.Username))
            {
                Send(session, Packet.Create(Command.ERROR, new { Message = "Can dang nhap truoc khi vao phong." }, "server"));
                return;
            }

            RoomJoinPayload payload = packet.ReadData<RoomJoinPayload>();
            if (payload == null || payload.RoomId <= 0)
            {
                Send(session, Packet.Create(Command.ERROR, new { Message = "ID phong khong hop le." }, "server"));
                return;
            }

            lock (_lockObj)
            {
                if (session.RoomId > 0)
                {
                    Send(session, Packet.Create(Command.ERROR, new { Message = "Ban da o trong phong." }, "server"));
                    return;
                }

                if (!_pendingRooms.TryGetValue(payload.RoomId, out PendingRoom pending))
                {
                    Send(session, Packet.Create(Command.ERROR, new { Message = "Phong khong ton tai hoac da bat dau." }, "server"));
                    SendRoomListTo(session);
                    return;
                }

                if (pending.Host == session)
                {
                    Send(session, Packet.Create(Command.ERROR, new { Message = "Khong the tu vao phong cua chinh ban." }, "server"));
                    return;
                }

                _pendingRooms.Remove(payload.RoomId);
                RemoveFromQueue(session);
                StartMatchInternal(pending.Host, session, payload.RoomId);
                BroadcastRoomList();
            }
        }

        private void HandleMove(ClientSession session, Packet packet)
        {
            MovePayload move = packet.ReadData<MovePayload>();
            if (move == null)
            {
                return;
            }

            GameRoom room;
            lock (_lockObj)
            {
                _rooms.TryGetValue(move.RoomId, out room);
            }

            if (room == null)
            {
                return;
            }

            if (room.IsWaitingForRematch)
            {
                Send(session, Packet.Create(Command.ERROR, new { Message = "Tran da ket thuc. Dang cho 15s de bat dau tran moi." }, "server"));
                return;
            }

            if (!room.TryApplyMove(session, move.Row, move.Col))
            {
                Send(session, Packet.Create(Command.ERROR, new { Message = "Nuoc di khong hop le." }, "server"));
                return;
            }

            var applied = new MovePayload
            {
                RoomId = move.RoomId,
                Row = move.Row,
                Col = move.Col,
                Symbol = room.Board[move.Row, move.Col].ToString()
            };

            Send(room.PlayerX, Packet.Create(Command.MOVE, applied, session.Username));
            Send(room.PlayerO, Packet.Create(Command.MOVE, applied, session.Username));

            if (room.CheckWin(move.Row, move.Col))
            {
                string winner = session.Username;
                string result = $"{winner} chien thang";
                FinishMatchWithWinner(room, winner, result);
                return;
            }

            if (room.MoveCount >= 15 * 15)
            {
                string result = "Hoa chung cuoc";
                var endPayload = new GameEndPayload
                {
                    RoomId = room.RoomId,
                    WinnerUsername = string.Empty,
                    ResultMessage = result
                };

                Send(room.PlayerX, Packet.Create(Command.GAME_END, endPayload, "server"));
                Send(room.PlayerO, Packet.Create(Command.GAME_END, endPayload, "server"));

                lock (_lockObj)
                {
                    room.IsWaitingForRematch = true;
                    room.RematchWaitStartTime = DateTime.Now;
                    Log("INFO", $"Tran phong {room.RoomId} ket thuc hoa. Dang cho 15s de tro choi tiep...");
                }
            }
        }

        private void HandleTurnTimeout(ClientSession session, Packet packet)
        {
            TurnTimeoutPayload timeout = packet.ReadData<TurnTimeoutPayload>();
            if (timeout == null)
            {
                return;
            }

            GameRoom room;
            lock (_lockObj)
            {
                _rooms.TryGetValue(timeout.RoomId, out room);
            }

            if (room == null)
            {
                return;
            }

            if (room.IsWaitingForRematch)
            {
                return;
            }

            bool isPlayerX = room.PlayerX == session;
            bool isPlayerO = room.PlayerO == session;
            if (!isPlayerX && !isPlayerO)
            {
                return;
            }

            char expectedSymbol = isPlayerX ? 'X' : 'O';
            if (room.CurrentTurn != expectedSymbol)
            {
                Send(session, Packet.Create(Command.ERROR, new { Message = "Khong phai luot cua ban de bao het gio." }, "server"));
                return;
            }

            ClientSession winnerSession = room.GetOpponent(session);
            string winner = winnerSession.Username;
            string result = $"{session.Username} het thoi gian suy nghi. {winner} chien thang";
            FinishMatchWithWinner(room, winner, result);
        }

        private void HandleGetRanking(ClientSession session, Packet packet)
        {
            RankingRequestPayload req = packet.ReadData<RankingRequestPayload>();
            int top = req?.Top ?? 200;
            if (top <= 0)
            {
                top = 200;
            }

            top = Math.Min(top, 1000);
            var users = _db.GetUsers(top);
            var payload = new RankingListPayload
            {
                Items = users
                    .Select((u, index) => new RankingItemPayload
                    {
                        Rank = index + 1,
                        Username = u.Username,
                        DisplayName = u.DisplayName,
                        Score = u.Score
                    })
                    .ToList()
            };

            Send(session, Packet.Create(Command.RANKING_LIST, payload, "server"));
        }

        private void HandleChat(ClientSession session, Packet packet)
        {
            ChatPayload chat = packet.ReadData<ChatPayload>();
            if (chat == null)
            {
                return;
            }

            GameRoom room;
            lock (_lockObj)
            {
                _rooms.TryGetValue(chat.RoomId, out room);
            }

            if (room == null)
            {
                return;
            }

            Send(room.PlayerX, Packet.Create(Command.CHAT, chat, session.Username));
            Send(room.PlayerO, Packet.Create(Command.CHAT, chat, session.Username));
            _db.AddChatLog(chat.RoomId, session.Username, chat.Message);
        }

        private void HandleLeaveRoom(ClientSession session)
        {
            if (session.RoomId <= 0)
            {
                return;
            }

            lock (_lockObj)
            {
                if (_rooms.TryGetValue(session.RoomId, out GameRoom room))
                {
                    ClientSession opponent = room.GetOpponent(session);
                    
                    // If in waiting state, the opponent can now join a new match
                    if (room.IsWaitingForRematch)
                    {
                        Send(opponent, Packet.Create(Command.OPPONENT_LEFT, new OpponentLeftPayload
                        {
                            RoomId = room.RoomId,
                            Message = $"{session.Username} da roi trong thoi gian cho. Dang tim doi thu moi cho ban..."
                        }, "server"));

                        opponent.RoomId = 0;
                        if (!string.IsNullOrWhiteSpace(opponent.Username) && !_quickMatchQueue.Contains(opponent))
                        {
                            _quickMatchQueue.Enqueue(opponent);
                        }

                        if (_quickMatchQueue.Count >= 2)
                        {
                            ClientSession p1 = _quickMatchQueue.Dequeue();
                            ClientSession p2 = _quickMatchQueue.Dequeue();
                            StartMatchInternal(p1, p2);
                        }
                    }
                    else
                    {
                        // Game is in progress
                        Send(opponent, Packet.Create(Command.OPPONENT_LEFT, new OpponentLeftPayload
                        {
                            RoomId = room.RoomId,
                            Message = $"{session.Username} da roi phong. Ban co the choi lai voi doi thu khac."
                        }, "server"));
                    }

                    _rooms.Remove(room.RoomId);
                    if (!room.IsWaitingForRematch)
                    {
                        opponent.RoomId = 0;
                    }
                }

                session.RoomId = 0;
                RemoveFromQueue(session);
                RemovePendingByHost(session);
                BroadcastRoomList();
            }
        }

        private void Disconnect(ClientSession session)
        {
            lock (_lockObj)
            {
                _sessions.Remove(session);
                RemoveFromQueue(session);
                RemovePendingByHost(session);
            }

            HandleLeaveRoom(session);

            try
            {
                session.Client.Close();
            }
            catch
            {
            }

            if (!string.IsNullOrEmpty(session.Username))
            {
                Log("WARN", $"{session.Username} da ngat ket noi.");
            }
        }

        private void RemoveFromQueue(ClientSession session)
        {
            if (!_quickMatchQueue.Contains(session))
            {
                return;
            }

            var keep = _quickMatchQueue.Where(s => s != session).ToArray();
            _quickMatchQueue.Clear();
            foreach (var item in keep)
            {
                _quickMatchQueue.Enqueue(item);
            }
        }

        private void RemovePendingByHost(ClientSession host)
        {
            var ids = _pendingRooms
                .Where(x => x.Value.Host == host)
                .Select(x => x.Key)
                .ToList();

            foreach (var id in ids)
            {
                _pendingRooms.Remove(id);
            }
        }

        private void SendRoomListTo(ClientSession session)
        {
            var payload = BuildRoomListPayload();
            Send(session, Packet.Create(Command.ROOM_LIST, payload, "server"));
        }

        private void BroadcastRoomList()
        {
            var payload = BuildRoomListPayload();
            var packet = Packet.Create(Command.ROOM_LIST, payload, "server");
            foreach (var s in _sessions)
            {
                if (!string.IsNullOrWhiteSpace(s.Username))
                {
                    Send(s, packet);
                }
            }
        }

        private RoomListPayload BuildRoomListPayload()
        {
            return new RoomListPayload
            {
                Rooms = _pendingRooms.Values
                    .OrderByDescending(r => r.CreatedAt)
                    .Select(r => new RoomInfo
                    {
                        RoomId = r.RoomId,
                        HostUsername = r.Host.Username,
                        HostDisplayName = r.Host.DisplayName,
                        Status = "Waiting"
                    })
                    .ToList()
            };
        }

        private void StartMatchInternal(ClientSession p1, ClientSession p2, int preferredRoomId = 0)
        {
            int roomId = preferredRoomId > 0 ? preferredRoomId : GenerateRoomId();
            var room = new GameRoom(roomId, p1, p2);
            _rooms[roomId] = room;
            p1.RoomId = roomId;
            p2.RoomId = roomId;

            var p1Payload = new MatchFoundPayload
            {
                RoomId = roomId,
                OpponentUsername = p2.Username,
                OpponentDisplayName = p2.DisplayName,
                OpponentAvatarBase64 = p2.AvatarBase64,
                YourSymbol = "X",
                YourTurn = true
            };

            var p2Payload = new MatchFoundPayload
            {
                RoomId = roomId,
                OpponentUsername = p1.Username,
                OpponentDisplayName = p1.DisplayName,
                OpponentAvatarBase64 = p1.AvatarBase64,
                YourSymbol = "O",
                YourTurn = false
            };

            Send(p1, Packet.Create(Command.MATCH_FOUND, p1Payload, "server"));
            Send(p2, Packet.Create(Command.MATCH_FOUND, p2Payload, "server"));
            Log("SUCCESS", $"Tao phong {roomId}: {p1.Username}(X) vs {p2.Username}(O)");
        }

        private void Send(ClientSession session, Packet packet)
        {
            try
            {
                session.Writer.WriteLine(packet.ToJson());
            }
            catch
            {
            }
        }

        private void FinishMatchWithWinner(GameRoom room, string winner, string result)
        {
            var endPayload = new GameEndPayload
            {
                RoomId = room.RoomId,
                WinnerUsername = winner,
                ResultMessage = result
            };

            Send(room.PlayerX, Packet.Create(Command.GAME_END, endPayload, "server"));
            Send(room.PlayerO, Packet.Create(Command.GAME_END, endPayload, "server"));

            _db.AddScore(winner, 10);
            _db.AddMatchHistory(room.RoomId, room.StartedAt, DateTime.Now, room.PlayerX.Username, room.PlayerO.Username, winner, result, room.MoveCount);
            SendAuthUpdate(room.PlayerX);
            SendAuthUpdate(room.PlayerO);

            lock (_lockObj)
            {
                room.IsWaitingForRematch = true;
                room.RematchWaitStartTime = DateTime.Now;
                Log("INFO", $"Tran phong {room.RoomId} ket thuc, winner={winner}. Dang cho 15s de tro choi tiep...");
            }
        }

        private void SendAuthUpdate(ClientSession session)
        {
            var user = _db.GetUserByUsername(session.Username);
            if (user == null)
            {
                return;
            }

            var response = new AuthResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                DisplayName = user.DisplayName,
                Score = user.Score,
                AvatarBase64 = user.AvatarBase64,
                Message = "Diem cua ban da duoc cap nhat." 
            };

            Send(session, Packet.Create(Command.AUTH_OK, response, "server"));
        }

        private void Log(string level, string message)
        {
            string line = $"[{level}] {message}";
            OnLog?.Invoke(line);
            _db.AddServerLog(level, message);
        }
    }

    internal class ClientSession
    {
        public TcpClient Client { get; }
        public StreamReader Reader { get; }
        public StreamWriter Writer { get; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string AvatarBase64 { get; set; } = string.Empty;
        public int RoomId { get; set; }

        public ClientSession(TcpClient client)
        {
            Client = client;
            var stream = client.GetStream();
            Reader = new StreamReader(stream, Encoding.UTF8);
            Writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
        }
    }

    internal class PendingRoom
    {
        public int RoomId { get; set; }
        public ClientSession Host { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    internal class GameRoom
    {
        public int RoomId { get; }
        public ClientSession PlayerX { get; }
        public ClientSession PlayerO { get; }
        public DateTime StartedAt { get; } = DateTime.Now;
        public int MoveCount { get; private set; }
        public char[,] Board { get; } = new char[15, 15];
        public char CurrentTurn { get; private set; } = 'X';
        public bool IsWaitingForRematch { get; set; } = false;
        public DateTime RematchWaitStartTime { get; set; }
        public const int REMATCH_WAIT_SECONDS = 15;

        public GameRoom(int roomId, ClientSession playerX, ClientSession playerO)
        {
            RoomId = roomId;
            PlayerX = playerX;
            PlayerO = playerO;
        }

        public bool TryApplyMove(ClientSession session, int row, int col)
        {
            if (row < 0 || row >= 15 || col < 0 || col >= 15)
            {
                return false;
            }

            char symbol = session == PlayerX ? 'X' : session == PlayerO ? 'O' : '\0';
            if (symbol == '\0' || symbol != CurrentTurn)
            {
                return false;
            }

            if (Board[row, col] != '\0')
            {
                return false;
            }

            Board[row, col] = symbol;
            MoveCount++;
            CurrentTurn = CurrentTurn == 'X' ? 'O' : 'X';
            return true;
        }

        public ClientSession GetOpponent(ClientSession session) => session == PlayerX ? PlayerO : PlayerX;

        public bool CheckWin(int row, int col)
        {
            char s = Board[row, col];
            if (s == '\0')
            {
                return false;
            }

            int[][] dirs =
            {
                new[] { 0, 1 },
                new[] { 1, 0 },
                new[] { 1, 1 },
                new[] { 1, -1 }
            };

            foreach (var dir in dirs)
            {
                int count = 1;
                count += Count(row, col, dir[0], dir[1], s);
                count += Count(row, col, -dir[0], -dir[1], s);
                if (count >= 5)
                {
                    return true;
                }
            }

            return false;
        }

        private int Count(int row, int col, int dr, int dc, char symbol)
        {
            int c = 0;
            int r = row + dr;
            int k = col + dc;
            while (r >= 0 && r < 15 && k >= 0 && k < 15 && Board[r, k] == symbol)
            {
                c++;
                r += dr;
                k += dc;
            }

            return c;
        }
    }
}