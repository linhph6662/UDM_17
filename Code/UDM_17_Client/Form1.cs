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
    int[,] matrix = new int[15, 15];

    // Các biến phân vai trò
    string myRole = ""; // Sẽ là "X" hoặc "O" do Server cấp
    string opponentRole = "";
    Color myColor, opponentColor;

    bool myTurn = false;
    bool isGameEnded = false;

    // Giao diện tự tạo (Khỏi cần kéo thả)
    Label lblStatus = new Label();
    Panel pnlBoard = new Panel();

    public Form1()
    {
        InitializeComponent();
        SetupUI(); // Gọi hàm trang trí giao diện
    }

    // Hàm "Make up" cho giao diện đẹp như game thật
    void SetupUI()
    {
        this.BackColor = Color.FromArgb(240, 244, 248); // Màu nền xanh xám hiện đại
        this.Size = new Size(500, 600); // Chỉnh lại size form cho vuông vắn

        // Cấu hình Nút Kết nối (button1 đã kéo thả sẵn)
        button1.FlatStyle = FlatStyle.Flat;
        button1.BackColor = Color.FromArgb(59, 130, 246); // Màu xanh dương
        button1.ForeColor = Color.White;
        button1.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        button1.Cursor = Cursors.Hand;

        // Cấu hình thanh Trạng thái (Label)
        lblStatus.Text = "Hãy bấm Kết nối Server";
        lblStatus.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        lblStatus.ForeColor = Color.DarkGray;
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(160, 20);
        this.Controls.Add(lblStatus);

        // Cấu hình khu vực Bàn cờ
        pnlBoard.Location = new Point(15, 70);
        pnlBoard.Size = new Size(450, 450);
        pnlBoard.Enabled = false; // Khóa bàn cờ lúc chưa vào game
        this.Controls.Add(pnlBoard);

        // Sinh 225 nút cờ siêu phẳng
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                Button btn = new Button()
                {
                    Width = 30,
                    Height = 30,
                    Location = new Point(j * 30, i * 30),
                    Tag = i + "|" + j,
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat, // Bỏ viền 3D nổi cục
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderColor = Color.LightGray; // Viền mỏng màu xám
                btn.Click += Btn_Click;
                pnlBoard.Controls.Add(btn);
                board[i, j] = btn;
            }
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        try
        {
            client = new TcpClient("127.0.0.1", 1234);
            stream = client.GetStream();

            button1.Enabled = false;
            button1.Text = "Đã vào phòng";
            button1.BackColor = Color.Gray;
            lblStatus.Text = "Đang chờ đối thủ...";

            Thread t = new Thread(ReceiveData) { IsBackground = true };
            t.Start();
        }
        catch { MessageBox.Show("Không kết nối được Server!"); }
    }

    private void Btn_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        if (isGameEnded || !myTurn || btn.Text != "") return;

        // Đánh dấu của mình
        btn.Text = myRole;
        btn.ForeColor = myColor;

        string[] pos = btn.Tag.ToString().Split('|');
        int r = int.Parse(pos[0]);
        int c = int.Parse(pos[1]);

        matrix[r, c] = 1;
        myTurn = false; // Đánh xong thì mất lượt
        lblStatus.Text = $"Bạn là [{myRole}] - Đang đợi đối thủ suy nghĩ...";
        lblStatus.ForeColor = Color.DarkOrange;

        // Gửi tọa độ lên Server
        byte[] data = Encoding.UTF8.GetBytes(btn.Tag.ToString());
        stream.Write(data, 0, data.Length);

        if (CheckWin(r, c, 1))
        {
            isGameEnded = true;
            MessageBox.Show("Chúc mừng! Bạn đã chiến thắng! 🎉");
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
                string[] parts = data.Split('|');

                // Nếu nhận được lệnh phân vai trò từ Server
                if (parts[0] == "ROLE")
                {
                    myRole = parts[1]; // Nhận "X" hoặc "O"
                    opponentRole = (myRole == "X") ? "O" : "X";
                    myColor = (myRole == "X") ? Color.Red : Color.Blue;
                    opponentColor = (opponentRole == "X") ? Color.Red : Color.Blue;

                    // Ai là X thì được đi trước
                    myTurn = (myRole == "X");

                    Invoke(new Action(() => {
                        lblStatus.Text = $"Bạn là [{myRole}] - " + (myTurn ? "Tới lượt bạn đánh!" : "Chờ đối thủ đánh trước...");
                        lblStatus.ForeColor = myColor;
                        pnlBoard.Enabled = true; // Bắt đầu game, mở khóa bàn cờ
                    }));
                }
                // Nếu nhận được tọa độ nước cờ
                else
                {
                    int r = int.Parse(parts[0]);
                    int c = int.Parse(parts[1]);

                    Invoke(new Action(() => {
                        board[r, c].Text = opponentRole;
                        board[r, c].ForeColor = opponentColor;
                        matrix[r, c] = 2;

                        myTurn = true; // Tới lượt mình
                        lblStatus.Text = $"Bạn là [{myRole}] - Tới lượt bạn đánh!";
                        lblStatus.ForeColor = myColor;

                        if (CheckWin(r, c, 2))
                        {
                            isGameEnded = true;
                            MessageBox.Show("Rất tiếc! Đối thủ đã chiến thắng! 😭");
                        }
                    }));
                }
            }
            catch { break; }
        }
    }

    // Hàm kiểm tra 5 con liên tiếp (Giữ nguyên như cũ)
    private bool CheckWin(int row, int col, int playerID)
    {
        int[][] directions = new int[][] { new int[] { 1, 0 }, new int[] { 0, 1 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
        foreach (var dir in directions)
        {
            int count = 1;
            int r = row + dir[0], c = col + dir[1];
            while (r >= 0 && r < 15 && c >= 0 && c < 15 && matrix[r, c] == playerID) { count++; r += dir[0]; c += dir[1]; }
            r = row - dir[0]; c = col - dir[1];
            while (r >= 0 && r < 15 && c >= 0 && c < 15 && matrix[r, c] == playerID) { count++; r -= dir[0]; c -= dir[1]; }
            if (count >= 5) return true;
        }
        return false;
    }
}