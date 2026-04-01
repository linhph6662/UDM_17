// Game Logic - Caro
// Author: Khoi
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

    // Nếu thắng thì không đổi lượt nữa
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
            currentPlayer = (currentPlayer == Player.X) ? Player.O : Player.X;
        }

        public bool CheckWin(int row, int col)
        {
            Player player = board[row, col];
            if (player == Player.None) return false;

            return CheckDirection(row, col, 1, 0, player) ||
                   CheckDirection(row, col, 0, 1, player) ||
                   CheckDirection(row, col, 1, 1, player) ||
                   CheckDirection(row, col, 1, -1, player);
        }

        private bool CheckDirection(int row, int col, int dRow, int dCol, Player player)
        {
            int count = 1;
            count += CountOneSide(row, col, dRow, dCol, player);
            count += CountOneSide(row, col, -dRow, -dCol, player);

            return count >= 5;
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
        public Player GetCell(int row, int col)
{
    return board[row, col];
}
    }
}
