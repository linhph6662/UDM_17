using Newtonsoft.Json;

namespace UDM_17.Core
{
    public enum Command
    {
        REGISTER,
        LOGIN,
        AUTH_OK,
        AUTH_FAIL,
        QUICK_MATCH,
        CREATE_ROOM,
        JOIN_ROOM,
        ROOM_CREATED,
        ROOM_LIST,
        MATCH_FOUND,
        MOVE,
        CHAT,
        GAME_END,
        OPPONENT_LEFT,
        ERROR,
        LEAVE_ROOM,
        GAME_END_WAIT,
        TURN_TIMEOUT,
        GET_RANKING,
        RANKING_LIST
    }

    public class Packet
    {
        public Command Cmd { get; set; }
        public string Data { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;

        public Packet()
        {
        }

        public Packet(Command cmd, string data, string sender = "")
        {
            Cmd = cmd; Data = data; Sender = sender;
        }

        public string ToJson() => JsonConvert.SerializeObject(this);
        public static Packet? FromJson(string json) => JsonConvert.DeserializeObject<Packet>(json);

        public static Packet Create<T>(Command cmd, T payload, string sender = "")
        {
            string jsonPayload = JsonConvert.SerializeObject(payload);
            return new Packet(cmd, jsonPayload, sender);
        }

        public T? ReadData<T>() => JsonConvert.DeserializeObject<T>(Data);
    }

    public class AuthRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int Score { get; set; }
        public string AvatarBase64 { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class MatchFoundPayload
    {
        public int RoomId { get; set; }
        public string OpponentUsername { get; set; } = string.Empty;
        public string OpponentDisplayName { get; set; } = string.Empty;
        public string OpponentAvatarBase64 { get; set; } = string.Empty;
        public string YourSymbol { get; set; } = "X";
        public bool YourTurn { get; set; }
    }

    public class MovePayload
    {
        public int RoomId { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public string Symbol { get; set; } = "X";
    }

    public class ChatPayload
    {
        public int RoomId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class GameEndPayload
    {
        public int RoomId { get; set; }
        public string WinnerUsername { get; set; } = string.Empty;
        public string ResultMessage { get; set; } = string.Empty;
    }

    public class OpponentLeftPayload
    {
        public int RoomId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class RoomJoinPayload
    {
        public int RoomId { get; set; }
    }

    public class RoomInfo
    {
        public int RoomId { get; set; }
        public string HostUsername { get; set; } = string.Empty;
        public string HostDisplayName { get; set; } = string.Empty;
        public string Status { get; set; } = "Waiting";
    }

    public class RoomListPayload
    {
        public List<RoomInfo> Rooms { get; set; } = new List<RoomInfo>();
    }

    public class GameEndWaitPayload
    {
        public int RoomId { get; set; }
        public int SecondsRemaining { get; set; }
    }

    public class TurnTimeoutPayload
    {
        public int RoomId { get; set; }
    }

    public class RankingRequestPayload
    {
        public int Top { get; set; } = 200;
    }

    public class RankingItemPayload
    {
        public int Rank { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int Score { get; set; }
    }

    public class RankingListPayload
    {
        public List<RankingItemPayload> Items { get; set; } = new List<RankingItemPayload>();
    }
}