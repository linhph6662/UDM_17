using System.Net;
using System.Net.Sockets;
using System.Text;
using UDM_17.Core;

namespace UDM_17.Server
{
    public class ServerManager
    {
        TcpListener listener;
        public List<TcpClient> Clients = new List<TcpClient>();
        public Action<string> OnLog;

        public void Start()
        {
            listener = new TcpListener(IPAddress.Any, 1234);
            listener.Start();
            Task.Run(() => {
                while (true)
                {
                    var client = listener.AcceptTcpClient();
                    Clients.Add(client);
                    OnLog?.Invoke($"[+] Kết nối mới: {client.Client.RemoteEndPoint}");
                    Task.Run(() => Receive(client));
                }
            });
        }

        void Receive(TcpClient client)
        {
            var ns = client.GetStream();
            byte[] buf = new byte[1024];
            while (true)
            {
                try
                {
                    int bytes = ns.Read(buf, 0, buf.Length);
                    if (bytes == 0) break;
                    string json = Encoding.UTF8.GetString(buf, 0, bytes);
                    Broadcast(json); // Gửi cho tất cả mọi người
                }
                catch { break; }
            }
            Clients.Remove(client);
        }

        void Broadcast(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            foreach (var c in Clients) c.GetStream().Write(data, 0, data.Length);
        }
    }
}