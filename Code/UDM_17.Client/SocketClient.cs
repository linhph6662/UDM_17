using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UDM_17.Client
{
    internal class SocketClient
    {
        private TcpClient client;
        private NetworkStream stream;

        // ===== EVENT gửi dữ liệu lên UI =====
        public event Action<string> OnMessageReceived;

        // ===============================
        // Kết nối server
        // ===============================
        public void Connect(string ip, int port)
        {
            client = new TcpClient();
            client.Connect(ip, port);
            stream = client.GetStream();

            // bắt đầu lắng nghe server
            _ = ReceiveLoop();
        }

        // ===============================
        // Gửi dữ liệu JSON
        // ===============================
        public void Send(object data)
        {
            if (stream == null) return;

            string json = JsonSerializer.Serialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            stream.Write(buffer, 0, buffer.Length);
        }

        // ===============================
        // Nhận dữ liệu realtime
        // ===============================
        private async Task ReceiveLoop()
        {
            byte[] buffer = new byte[4096];

            while (client.Connected)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                    break;

                string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // gửi dữ liệu lên UI
                OnMessageReceived?.Invoke(json);
            }
        }

        // ===============================
        // Ngắt kết nối
        // ===============================
        public void Disconnect()
        {
            stream?.Close();
            client?.Close();
        }
    }
}
