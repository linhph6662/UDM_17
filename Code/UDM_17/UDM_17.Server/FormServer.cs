using System.Net;
using System.Net.Sockets;
using System.Text;
using UDM_17.Core;

namespace UDM_17.Server
{
    public partial class FormServer : Form
    {
        TcpListener listener;
        List<TcpClient> clients = new List<TcpClient>();

        public FormServer()
        {
            InitializeComponent();
            this.Text = "UDM_17 - Caro Server Manager";
            btnStartServer.Click += (s, e) => StartServer();
            // Cho phép cập nhật UI từ luồng khác (Socket thread)
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        void StartServer()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 1234);
                listener.Start();
                lstLog.Items.Add("[SYSTEM] Server đang lắng nghe tại cổng 1234...");
                btnStartServer.Enabled = false;
                btnStartServer.Text = "SERVER ONLINE";

                Task.Run(() => {
                    while (true)
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        clients.Add(client);
                        lstLog.Items.Add($"[+] Kết nối mới từ: {client.Client.RemoteEndPoint}");
                        Task.Run(() => HandleClient(client));
                    }
                });
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void HandleClient(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] buf = new byte[2048];
            while (true)
            {
                try
                {
                    int bytes = ns.Read(buf, 0, buf.Length);
                    if (bytes == 0) break;
                    string json = Encoding.UTF8.GetString(buf, 0, bytes);
                    Broadcast(json);
                }
                catch { break; }
            }
            clients.Remove(client);
            lstLog.Items.Add($"[-] Một người chơi đã thoát.");
        }

        void Broadcast(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            foreach (var c in clients)
            {
                try { c.GetStream().Write(data, 0, data.Length); } catch { }
            }
        }
    }
}