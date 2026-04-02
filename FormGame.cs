using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaroGame
{
    public partial class FormGame : Form
    {
        private const int BOARD_SIZE = 10;
        private Button[,] board = new Button[BOARD_SIZE, BOARD_SIZE];
        private bool isXTurn = true;

        public FormGame()
        {
            InitializeComponent();
            CreateBoard();
        }

        private void CreateBoard()
        {
            int size = 40;

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    Button btn = new Button();
                    btn.Width = size;
                    btn.Height = size;
                    btn.Left = j * size;
                    btn.Top = i * size;
                    btn.Font = new Font("Arial", 14, FontStyle.Bold);
                    btn.Tag = new Point(i, j);
                    btn.Click += Btn_Click;

                    board[i, j] = btn;
                    this.Controls.Add(btn);
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            if (btn.Text != "") return;

            if (isXTurn)
            {
                btn.Text = "X";
                btn.ForeColor = Color.Red;
            }
            else
            {
                btn.Text = "O";
                btn.ForeColor = Color.Blue;
            }

            isXTurn = !isXTurn;
        }

        private void FormGame_Load(object sender, EventArgs e)
        {

        }
    }
}