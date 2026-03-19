using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UDM_17_Client;

public partial class Form1 : Form
{
    TcpClient client;
    NetworkStream stream;
    Button[,] board = new Button[15, 15];

    public Form1()
    {
        InitializeComponent();
        CreateBoard();
    }

    void CreateBoard()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                Button btn = new Button()
                {
                    Width = 30,
                    Height = 30,
                    Location = new Point(j * 30, i * 30 + 50), // Dịch xuống 50px chừa chỗ cho nút Kết nối
                    Tag = i + "|" + j
                };
                btn.Click += Btn_Click;
                this.Controls.Add(btn); // Thêm thẳng vào Form luôn cho lẹ, khỏi cần Panel
                board[i, j] = btn;
            }
        }
    }

    private void Btn_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        if (btn.Text != "" || client == null) return;

        btn.Text = "X";
        btn.ForeColor = Color.Red;

        byte[] data = Encoding.UTF8.GetBytes(btn.Tag.ToString());
        stream.Write(data, 0, data.Length);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        try
        {
            client = new TcpClient("127.0.0.1", 1234);
            stream = client.GetStream();

            button1.Enabled = false;
            button1.Text = "Đã kết nối";

            Thread t = new Thread(ReceiveData);
            t.IsBackground = true;
            t.Start();
        }
        catch
        {
            MessageBox.Show("Không kết nối được Server!");
        }
    }

    void ReceiveData()
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            try
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);
                if (bytes == 0) break;

                string data = Encoding.UTF8.GetString(buffer, 0, bytes);
                string[] pos = data.Split('|');
                int i = int.Parse(pos[0]);
                int j = int.Parse(pos[1]);

                Invoke(new Action(() => {
                    board[i, j].Text = "O";
                    board[i, j].ForeColor = Color.Blue;
                }));
            }
            catch { break; }
        }
    }
}