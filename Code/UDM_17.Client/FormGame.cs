using System.Net.Sockets;
using System.Text;
using UDM_17.Core;

namespace UDM_17.Client
{
    public partial class FormGame : Form
    {
        TcpClient client;
        NetworkStream ns;
        Button[,] banCo = new Button[15, 15];
        GameLogic logic = new GameLogic();
        string myID = Guid.NewGuid().ToString().Substring(0, 4);

        public FormGame()
        {
            InitializeComponent();
            this.Text = "UDM_17 Caro - ID: " + myID;

            btnConnect.Click += (s, e) => {
                try
                {
                    client = new TcpClient("127.0.0.1", 1234);
                    ns = client.GetStream();
                    btnConnect.Enabled = false; 
                    btnConnect.Text = "ĐÃ KẾT NỐI";
                    lblStatus.Text = "Đã kết nối tới Server.";
                    Task.Run(() => Listen());
                }
                catch { MessageBox.Show("Không tìm thấy Server!"); }
            };

            DrawBoard();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        void DrawBoard()
        {
            int cellSize = 30;
            for (int r = 0; r < 15; r++)
            {
                for (int c = 0; c < 15; c++)
                {
                    Button b = new Button { Width = cellSize, Height = cellSize, Location = new Point(c * cellSize, r * cellSize), FlatStyle = FlatStyle.Flat, Tag = $"{r}|{c}" };
                    b.Click += (s, e) => SendMove((Button)s);
                    banCo[r, c] = b;
                    pnlChessBoard.Controls.Add(b);
                }
            }
        }

        void SendMove(Button btn)
        {
            if (ns == null) return;
            var p = new Packet(Command.MOVE, btn.Tag.ToString(), myID);
            byte[] d = Encoding.UTF8.GetBytes(p.ToJson());
            ns.Write(d, 0, d.Length);
        }

        void Listen()
        {
            byte[] buf = new byte[2048];
            while (true)
            {
                try
                {
                    int b = ns.Read(buf, 0, buf.Length);
                    var p = Packet.FromJson(Encoding.UTF8.GetString(buf, 0, b));
                    if (p.Cmd == Command.MOVE)
                    {
                        string[] pos = p.Data.Split('|');
                        int r = int.Parse(pos[0]), c = int.Parse(pos[1]);
                        int playerNum = (p.Sender == myID) ? 1 : 2;

                        logic.Board[r, c] = playerNum;
                        this.Invoke(new Action(() => {
                            banCo[r, c].Text = (playerNum == 1) ? "X" : "O";
                            banCo[r, c].ForeColor = (playerNum == 1) ? Color.Blue : Color.Red;
                            banCo[r, c].Enabled = false;
                            if (logic.CheckWin(r, c, playerNum)) MessageBox.Show("Người chơi " + p.Sender + " đã thắng!");
                        }));
                    }
                }
                catch { break; }
            }
        }
    }
}