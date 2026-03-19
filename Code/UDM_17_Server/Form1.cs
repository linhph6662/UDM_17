using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UDM_17_Server;

public partial class Form1 : Form
{
    TcpListener server;
    TcpClient client1, client2;

    public Form1() { InitializeComponent(); }
    private void Form1_Load(object sender, EventArgs e) { }

    private void button1_Click(object sender, EventArgs e)
    {
        Thread t = new Thread(StartServer) { IsBackground = true };
        t.Start();
        button1.Enabled = false;
    }

    void StartServer()
    {
        server = new TcpListener(IPAddress.Any, 1234);
        server.Start();
        Log("Server đã bật. Đang chờ người chơi 1...");

        client1 = server.AcceptTcpClient();
        Log("Player 1 đã kết nối! Đang chờ người chơi 2...");

        client2 = server.AcceptTcpClient();
        Log("Player 2 đã kết nối! Đã đủ 2 người, trận đấu bắt đầu.");

        // TRỌNG TÂM Ở ĐÂY: Server phân vai trò cho 2 Client
        SendToClient(client1, "ROLE|X");
        SendToClient(client2, "ROLE|O");

        new Thread(() => Handle(client1, client2)) { IsBackground = true }.Start();
        new Thread(() => Handle(client2, client1)) { IsBackground = true }.Start();
    }

    // Hàm gửi tin nhắn chuyên dụng
    void SendToClient(TcpClient client, string msg)
    {
        byte[] data = Encoding.UTF8.GetBytes(msg);
        client.GetStream().Write(data, 0, data.Length);
    }

    void Handle(TcpClient sender, TcpClient receiver)
    {
        var stream = sender.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            try
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);
                if (bytes == 0) break;

                // Nhận tọa độ từ người này, quăng y nguyên sang người kia
                string data = Encoding.UTF8.GetString(buffer, 0, bytes);
                Log("Nước đi: " + data);
                SendToClient(receiver, data);
            }
            catch { Log("Cảnh báo: Một Client đã thoát."); break; }
        }
    }

    void Log(string msg)
    {
        Invoke(new Action(() => { textBox1.AppendText(msg + Environment.NewLine); }));
    }
}