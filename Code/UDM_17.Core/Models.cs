using Newtonsoft.Json;

namespace UDM_17.Core
{
    public enum Command { LOGIN, MOVE, MESSAGE, RESTART }

    public class Packet
    {
        public Command Cmd { get; set; }
        public string Data { get; set; }
        public string Sender { get; set; }

        public Packet(Command cmd, string data, string sender = "")
        {
            Cmd = cmd; Data = data; Sender = sender;
        }

        public string ToJson() => JsonConvert.SerializeObject(this);
        public static Packet? FromJson(string json) => JsonConvert.DeserializeObject<Packet>(json);
    }
}