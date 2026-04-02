// Game Logic - Caro
// Chức năng: xử lý bàn cờ, lượt chơi, kiểm tra thắng

using System;

namespace CaroGame
{
    public enum Player
    {
        None,
        X,
        O
    }

    public class GameLogic
    {
        private readonly int size;
        private Player[,] board;
        private Player currentPlayer;
        private int moveCount;

        public GameLogic(int boardSize = 15)
        {
            size = boardSize;
            board = new Player[size, size];
            currentPlayer = Player.X;
            moveCount = 0;
        }

        public Player CurrentPlayer => currentPlayer;

        // Không expose trực tiếp board để tránh bị sửa từ ngoài
        public Player GetCell(int row, int col)
        {
            return board[row, col];
        }

        public bool MakeMove(int row, int col)
        {
            if (!IsValidMove(row, col))
                return false;

            board[row, col] = currentPlayer;
            moveCount++;

            // Tự động đổi lượt nếu chưa thắng
            if (!CheckWin(row, col))
            {
                SwitchTurn();
            }

            return true;
        }

        public bool IsValidMove(int row, int col)
        {
            return row >= 0 && row < size &&
                   col >= 0 && col < size &&
                   board[row, col] == Player.None;
        }

        public void SwitchTurn()
        {
            currentPlayer = (currentPlayer == Player.X)
                ? Player.O
                : Player.X;
        }

        public bool CheckWin(int row, int col)
        {
            Player player = board[row, col];

            if (player == Player.None)
                return false;

            return CheckDirection(row, col, 1, 0, player) ||   // dọc
                   CheckDirection(row, col, 0, 1, player) ||   // ngang
                   CheckDirection(row, col, 1, 1, player) ||   // chéo \
                   CheckDirection(row, col, 1, -1, player);    // chéo /
        }

        private bool CheckDirection(int row, int col, int dRow, int dCol, Player player)
        {
            int count = 1;

            int count1 = CountOneSide(row, col, dRow, dCol, player);
            int count2 = CountOneSide(row, col, -dRow, -dCol, player);

            count += count1 + count2;

            // Kiểm tra bị chặn 2 đầu
            bool blocked1 = IsBlocked(row + (count1 + 1) * dRow, col + (count1 + 1) * dCol);
            bool blocked2 = IsBlocked(row - (count2 + 1) * dRow, col - (count2 + 1) * dCol);

            return count >= 5 && !(blocked1 && blocked2);
        }

        private int CountOneSide(int row, int col, int dRow, int dCol, Player player)
        {
            int count = 0;

            int r = row + dRow;
            int c = col + dCol;

            while (r >= 0 && r < size &&
                   c >= 0 && c < size &&
                   board[r, c] == player)
            {
                count++;
                r += dRow;
                c += dCol;
            }

            return count;
        }

        // Hàm kiểm tra bị chặn
        private bool IsBlocked(int row, int col)
        {
            if (row < 0 || row >= size || col < 0 || col >= size)
                return true; // ra ngoài bàn coi như bị chặn

            return board[row, col] != Player.None;
        }

        public bool IsDraw()
        {
            return moveCount == size * size;
        }

        public void Reset()
        {
            board = new Player[size, size];
            currentPlayer = Player.X;
            moveCount = 0;
        }
    }
}
