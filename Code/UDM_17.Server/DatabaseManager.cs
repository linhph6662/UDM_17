using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace UDM_17.Server
{
    // ============================================================
    // Model: Thông tin người chơi lưu trong DB
    // ============================================================
    public class PlayerRecord
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }   // Tên (hoặc ID ngắn) của người chơi
        public int TotalWins { get; set; }
        public int TotalLosses { get; set; }
        public int TotalDraws { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ============================================================
    // Model: Kết quả một trận đấu
    // ============================================================
    public class MatchRecord
    {
        public int Id { get; set; }
        public string PlayerX { get; set; }       // Người chơi quân X
        public string PlayerO { get; set; }       // Người chơi quân O
        public string Winner { get; set; }        // "X", "O" hoặc "DRAW"
        public int TotalMoves { get; set; }       // Tổng số nước đi
        public DateTime PlayedAt { get; set; }    // Thời điểm kết thúc trận
    }

    // ============================================================
    // DatabaseManager – quản lý SQLite cho Server Caro
    // ============================================================
    internal class DatabaseManager
    {
        private readonly string _connectionString;

        /// <summary>
        /// Khởi tạo DatabaseManager.
        /// File cơ sở dữ liệu mặc định: caro_game.db (cùng thư mục exe).
        /// </summary>
        public DatabaseManager(string dbFileName = "caro_game.db")
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFileName);
            _connectionString = $"Data Source={dbPath};Version=3;";

            // Tạo bảng nếu chưa tồn tại
            InitializeDatabase();
        }

        // ----------------------------------------------------------
        //  Khởi tạo cơ sở dữ liệu – tạo bảng Players & Matches
        // ----------------------------------------------------------
        private void InitializeDatabase()
        {
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sqlPlayers = @"
                CREATE TABLE IF NOT EXISTS Players (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    PlayerName  TEXT    NOT NULL UNIQUE,
                    TotalWins   INTEGER DEFAULT 0,
                    TotalLosses INTEGER DEFAULT 0,
                    TotalDraws  INTEGER DEFAULT 0,
                    CreatedAt   TEXT    DEFAULT (datetime('now','localtime'))
                );";

            string sqlMatches = @"
                CREATE TABLE IF NOT EXISTS Matches (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    PlayerX     TEXT    NOT NULL,
                    PlayerO     TEXT    NOT NULL,
                    Winner      TEXT    NOT NULL,
                    TotalMoves  INTEGER DEFAULT 0,
                    PlayedAt    TEXT    DEFAULT (datetime('now','localtime'))
                );";

            using (var cmd = new SQLiteCommand(sqlPlayers, conn))
                cmd.ExecuteNonQuery();

            using (var cmd = new SQLiteCommand(sqlMatches, conn))
                cmd.ExecuteNonQuery();
        }

        // ==========================================================
        //  1. QUẢN LÝ NGƯỜI CHƠI (Players)
        // ==========================================================

        /// <summary>
        /// Thêm hoặc lấy người chơi theo tên.
        /// Nếu chưa tồn tại → INSERT, nếu đã có → bỏ qua.
        /// </summary>
        public void AddPlayerIfNotExists(string playerName)
        {
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sql = "INSERT OR IGNORE INTO Players (PlayerName) VALUES (@name);";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", playerName);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Lấy thông tin một người chơi theo tên.
        /// Trả về null nếu không tìm thấy.
        /// </summary>
        public PlayerRecord GetPlayer(string playerName)
        {
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM Players WHERE PlayerName = @name;";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", playerName);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new PlayerRecord
                {
                    Id = reader.GetInt32(0),
                    PlayerName = reader.GetString(1),
                    TotalWins = reader.GetInt32(2),
                    TotalLosses = reader.GetInt32(3),
                    TotalDraws = reader.GetInt32(4),
                    CreatedAt = DateTime.Parse(reader.GetString(5))
                };
            }
            return null;
        }

        /// <summary>
        /// Lấy danh sách tất cả người chơi (xếp theo số trận thắng giảm dần).
        /// </summary>
        public List<PlayerRecord> GetAllPlayers()
        {
            var list = new List<PlayerRecord>();
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM Players ORDER BY TotalWins DESC;";
            using var cmd = new SQLiteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new PlayerRecord
                {
                    Id = reader.GetInt32(0),
                    PlayerName = reader.GetString(1),
                    TotalWins = reader.GetInt32(2),
                    TotalLosses = reader.GetInt32(3),
                    TotalDraws = reader.GetInt32(4),
                    CreatedAt = DateTime.Parse(reader.GetString(5))
                });
            }
            return list;
        }

        // ==========================================================
        //  2. LƯU KẾT QUẢ TRẬN ĐẤU (Matches)
        // ==========================================================

        /// <summary>
        /// Lưu kết quả một trận đấu vào cơ sở dữ liệu.
        /// Đồng thời cập nhật thống kê Win / Loss / Draw cho cả hai người chơi.
        /// </summary>
        /// <param name="playerX">Tên người chơi X</param>
        /// <param name="playerO">Tên người chơi O</param>
        /// <param name="winner">"X", "O" hoặc "DRAW"</param>
        /// <param name="totalMoves">Tổng số nước đi trong trận</param>
        public void SaveMatchResult(string playerX, string playerO, string winner, int totalMoves = 0)
        {
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            // Đảm bảo cả hai người chơi đã tồn tại trong bảng Players
            EnsurePlayerExists(conn, playerX);
            EnsurePlayerExists(conn, playerO);

            // ---- Bắt đầu Transaction để đảm bảo tính toàn vẹn ----
            using var transaction = conn.BeginTransaction();
            try
            {
                // 1) INSERT vào bảng Matches
                string sqlInsert = @"
                    INSERT INTO Matches (PlayerX, PlayerO, Winner, TotalMoves)
                    VALUES (@px, @po, @winner, @moves);";
                using (var cmd = new SQLiteCommand(sqlInsert, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@px", playerX);
                    cmd.Parameters.AddWithValue("@po", playerO);
                    cmd.Parameters.AddWithValue("@winner", winner);
                    cmd.Parameters.AddWithValue("@moves", totalMoves);
                    cmd.ExecuteNonQuery();
                }

                // 2) Cập nhật thống kê cho từng người chơi
                if (winner == "DRAW")
                {
                    UpdatePlayerStat(conn, transaction, playerX, "TotalDraws");
                    UpdatePlayerStat(conn, transaction, playerO, "TotalDraws");
                }
                else if (winner == "X")
                {
                    UpdatePlayerStat(conn, transaction, playerX, "TotalWins");
                    UpdatePlayerStat(conn, transaction, playerO, "TotalLosses");
                }
                else if (winner == "O")
                {
                    UpdatePlayerStat(conn, transaction, playerO, "TotalWins");
                    UpdatePlayerStat(conn, transaction, playerX, "TotalLosses");
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // ==========================================================
        //  3. XEM LỊCH SỬ TRẬN ĐẤU (Match History)
        // ==========================================================

        /// <summary>
        /// Lấy toàn bộ lịch sử trận đấu (mới nhất trước).
        /// </summary>
        public List<MatchRecord> GetAllMatches()
        {
            var list = new List<MatchRecord>();
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM Matches ORDER BY PlayedAt DESC;";
            using var cmd = new SQLiteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(ReadMatchRecord(reader));
            }
            return list;
        }

        /// <summary>
        /// Lấy lịch sử trận đấu của một người chơi cụ thể.
        /// </summary>
        public List<MatchRecord> GetMatchesByPlayer(string playerName)
        {
            var list = new List<MatchRecord>();
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sql = @"
                SELECT * FROM Matches
                WHERE PlayerX = @name OR PlayerO = @name
                ORDER BY PlayedAt DESC;";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", playerName);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(ReadMatchRecord(reader));
            }
            return list;
        }

        /// <summary>
        /// Lấy N trận đấu gần nhất.
        /// </summary>
        public List<MatchRecord> GetRecentMatches(int count = 10)
        {
            var list = new List<MatchRecord>();
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM Matches ORDER BY PlayedAt DESC LIMIT @limit;";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@limit", count);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(ReadMatchRecord(reader));
            }
            return list;
        }

        // ==========================================================
        //  4. THỐNG KÊ NHANH
        // ==========================================================

        /// <summary>
        /// Đếm tổng số trận đã diễn ra.
        /// </summary>
        public int GetTotalMatchCount()
        {
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sql = "SELECT COUNT(*) FROM Matches;";
            using var cmd = new SQLiteCommand(sql, conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Đếm tổng số người chơi đã đăng ký.
        /// </summary>
        public int GetTotalPlayerCount()
        {
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sql = "SELECT COUNT(*) FROM Players;";
            using var cmd = new SQLiteCommand(sql, conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // ==========================================================
        //  HELPER METHODS (private)
        // ==========================================================

        /// <summary>
        /// Đảm bảo người chơi tồn tại trong bảng Players (dùng trong transaction).
        /// </summary>
        private void EnsurePlayerExists(SQLiteConnection conn, string playerName)
        {
            string sql = "INSERT OR IGNORE INTO Players (PlayerName) VALUES (@name);";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", playerName);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Tăng 1 đơn vị cho cột thống kê (TotalWins / TotalLosses / TotalDraws).
        /// </summary>
        private void UpdatePlayerStat(SQLiteConnection conn, SQLiteTransaction trans,
                                       string playerName, string column)
        {
            // column chỉ nhận giá trị cố định nên an toàn khi nối chuỗi
            string sql = $"UPDATE Players SET {column} = {column} + 1 WHERE PlayerName = @name;";
            using var cmd = new SQLiteCommand(sql, conn, trans);
            cmd.Parameters.AddWithValue("@name", playerName);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Đọc một MatchRecord từ SQLiteDataReader.
        /// </summary>
        private MatchRecord ReadMatchRecord(SQLiteDataReader reader)
        {
            return new MatchRecord
            {
                Id = reader.GetInt32(0),
                PlayerX = reader.GetString(1),
                PlayerO = reader.GetString(2),
                Winner = reader.GetString(3),
                TotalMoves = reader.GetInt32(4),
                PlayedAt = DateTime.Parse(reader.GetString(5))
            };
        }
    }
}
