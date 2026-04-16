using Microsoft.Data.Sqlite;

namespace UDM_17.Server
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string dbFilePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath) ?? AppContext.BaseDirectory);
            _connectionString = $"Data Source={dbFilePath}";
            Initialize();
        }

        private SqliteConnection OpenConnection()
        {
            var conn = new SqliteConnection(_connectionString);
            conn.Open();
            return conn;
        }

        public void Initialize()
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Users (
    UserId TEXT PRIMARY KEY,
    Username TEXT NOT NULL UNIQUE,
    DisplayName TEXT NOT NULL,
    Password TEXT NOT NULL,
    Score INTEGER NOT NULL DEFAULT 0,
    AvatarBase64 TEXT NOT NULL DEFAULT '',
    CreatedAt TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS ServerLogs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CreatedAt TEXT NOT NULL,
    Level TEXT NOT NULL,
    Message TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS MatchHistory (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    RoomId INTEGER NOT NULL,
    StartedAt TEXT NOT NULL,
    EndedAt TEXT NOT NULL,
    PlayerX TEXT NOT NULL,
    PlayerO TEXT NOT NULL,
    Winner TEXT NOT NULL,
    ResultMessage TEXT NOT NULL,
    MoveCount INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS ChatHistory (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    RoomId INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    Sender TEXT NOT NULL,
    Message TEXT NOT NULL
);
";
            cmd.ExecuteNonQuery();
            EnsureDefaultUsers();
        }

        private void EnsureDefaultUsers()
        {
            if (GetTotalUsers() > 0)
            {
                return;
            }

            Register("admin", "admin", "Administrator");
            Register("guest", "guest", "Guest");
        }

        public (bool Success, string Message) Register(string username, string password, string displayName)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Username va password khong duoc rong.");
            }

            using var conn = OpenConnection();
            using var check = conn.CreateCommand();
            check.CommandText = "SELECT COUNT(1) FROM Users WHERE Username = $u";
            check.Parameters.AddWithValue("$u", username.Trim());
            long exists = (long)(check.ExecuteScalar() ?? 0L);
            if (exists > 0)
            {
                return (false, "Username da ton tai.");
            }

            using var insert = conn.CreateCommand();
            insert.CommandText = @"
INSERT INTO Users (UserId, Username, DisplayName, Password, CreatedAt)
VALUES ($id, $u, $d, $p, $t);";
            insert.Parameters.AddWithValue("$id", Guid.NewGuid().ToString());
            insert.Parameters.AddWithValue("$u", username.Trim());
            insert.Parameters.AddWithValue("$d", string.IsNullOrWhiteSpace(displayName) ? username.Trim() : displayName.Trim());
            insert.Parameters.AddWithValue("$p", password);
            insert.Parameters.AddWithValue("$t", DateTime.UtcNow.ToString("o"));
            insert.ExecuteNonQuery();

            return (true, "Dang ky thanh cong.");
        }

        public UserRecord Login(string username, string password)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT UserId, Username, DisplayName, Score, AvatarBase64
FROM Users
WHERE Username = $u AND Password = $p
LIMIT 1;";
            cmd.Parameters.AddWithValue("$u", username.Trim());
            cmd.Parameters.AddWithValue("$p", password);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new UserRecord
            {
                UserId = reader.GetString(0),
                Username = reader.GetString(1),
                DisplayName = reader.GetString(2),
                Score = reader.GetInt32(3),
                AvatarBase64 = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
            };
        }

        public UserRecord GetUserByUsername(string username)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT UserId, Username, DisplayName, Score, AvatarBase64
FROM Users
WHERE Username = $u
LIMIT 1;";
            cmd.Parameters.AddWithValue("$u", username.Trim());

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new UserRecord
            {
                UserId = reader.GetString(0),
                Username = reader.GetString(1),
                DisplayName = reader.GetString(2),
                Score = reader.GetInt32(3),
                AvatarBase64 = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
            };
        }

        public void AddServerLog(string level, string message)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO ServerLogs (CreatedAt, Level, Message)
VALUES ($t, $l, $m);";
            cmd.Parameters.AddWithValue("$t", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$l", level);
            cmd.Parameters.AddWithValue("$m", message);
            cmd.ExecuteNonQuery();
        }

        public void AddChatLog(int roomId, string sender, string message)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO ChatHistory (RoomId, CreatedAt, Sender, Message)
VALUES ($r, $t, $s, $m);";
            cmd.Parameters.AddWithValue("$r", roomId);
            cmd.Parameters.AddWithValue("$t", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$s", sender);
            cmd.Parameters.AddWithValue("$m", message);
            cmd.ExecuteNonQuery();
        }

        public void AddMatchHistory(int roomId, DateTime startedAt, DateTime endedAt, string playerX, string playerO, string winner, string resultMessage, int moveCount)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO MatchHistory (RoomId, StartedAt, EndedAt, PlayerX, PlayerO, Winner, ResultMessage, MoveCount)
VALUES ($room, $start, $end, $x, $o, $w, $msg, $count);";
            cmd.Parameters.AddWithValue("$room", roomId);
            cmd.Parameters.AddWithValue("$start", startedAt.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$end", endedAt.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$x", playerX);
            cmd.Parameters.AddWithValue("$o", playerO);
            cmd.Parameters.AddWithValue("$w", winner);
            cmd.Parameters.AddWithValue("$msg", resultMessage);
            cmd.Parameters.AddWithValue("$count", moveCount);
            cmd.ExecuteNonQuery();
        }

        public void AddScore(string username, int delta)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Users SET Score = Score + $d WHERE Username = $u";
            cmd.Parameters.AddWithValue("$d", delta);
            cmd.Parameters.AddWithValue("$u", username);
            cmd.ExecuteNonQuery();
        }

        public List<ServerLogItem> GetRecentLogs(int top = 200)
        {
            var items = new List<ServerLogItem>();
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT CreatedAt, Level, Message
FROM ServerLogs
ORDER BY Id DESC
LIMIT $top;";
            cmd.Parameters.AddWithValue("$top", top);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new ServerLogItem
                {
                    CreatedAt = reader.GetString(0),
                    Level = reader.GetString(1),
                    Message = reader.GetString(2)
                });
            }

            return items;
        }

        public List<ServerLogItem> GetServerLogs(DateTime? from = null, DateTime? to = null, int top = 500)
        {
            var items = new List<ServerLogItem>();
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            var sql = @"
SELECT CreatedAt, Level, Message
FROM ServerLogs";

            var clauses = new List<string>();
            if (from.HasValue)
            {
                clauses.Add("CreatedAt >= $from");
                cmd.Parameters.AddWithValue("$from", from.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            if (to.HasValue)
            {
                clauses.Add("CreatedAt <= $to");
                cmd.Parameters.AddWithValue("$to", to.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            if (clauses.Count > 0)
            {
                sql += "\r\nWHERE " + string.Join(" AND ", clauses);
            }

            sql += "\r\nORDER BY Id DESC\r\nLIMIT $top;";
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("$top", top);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new ServerLogItem
                {
                    CreatedAt = reader.GetString(0),
                    Level = reader.GetString(1),
                    Message = reader.GetString(2)
                });
            }

            return items;
        }

        public List<MatchHistoryItem> GetRecentMatches(int top = 100)
        {
            var items = new List<MatchHistoryItem>();
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT RoomId, StartedAt, EndedAt, PlayerX, PlayerO, Winner, ResultMessage, MoveCount
FROM MatchHistory
ORDER BY Id DESC
LIMIT $top;";
            cmd.Parameters.AddWithValue("$top", top);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new MatchHistoryItem
                {
                    RoomId = reader.GetInt32(0),
                    StartedAt = reader.GetString(1),
                    EndedAt = reader.GetString(2),
                    PlayerX = reader.GetString(3),
                    PlayerO = reader.GetString(4),
                    Winner = reader.GetString(5),
                    ResultMessage = reader.GetString(6),
                    MoveCount = reader.GetInt32(7)
                });
            }

            return items;
        }

        public int GetTotalUsers()
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(1) FROM Users";
            return Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
        }

        public List<UserListItem> GetUsers(int top = 200)
        {
            var items = new List<UserListItem>();
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT Username, DisplayName, Score
FROM Users
ORDER BY Score DESC, Username ASC
LIMIT $top;";
            cmd.Parameters.AddWithValue("$top", top);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new UserListItem
                {
                    Username = reader.GetString(0),
                    DisplayName = reader.GetString(1),
                    Score = reader.GetInt32(2)
                });
            }

            return items;
        }
    }

    public class UserRecord
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int Score { get; set; }
        public string AvatarBase64 { get; set; } = string.Empty;
    }

    public class ServerLogItem
    {
        public string CreatedAt { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class MatchHistoryItem
    {
        public int RoomId { get; set; }
        public string StartedAt { get; set; } = string.Empty;
        public string EndedAt { get; set; } = string.Empty;
        public string PlayerX { get; set; } = string.Empty;
        public string PlayerO { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public string ResultMessage { get; set; } = string.Empty;
        public int MoveCount { get; set; }
    }

    public class UserListItem
    {
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int Score { get; set; }
    }
}
