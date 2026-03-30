namespace UDM_17.Client
{
    public class GameLogic
    {
        public int[,] Board = new int[15, 15]; 

        public bool CheckWin(int r, int c, int p)
        {
            int[] dr = { 0, 1, 1, 1 }, dc = { 1, 0, 1, -1 };
            for (int i = 0; i < 4; i++)
            {
                int count = 1;
                count += CountDirection(r, c, dr[i], dc[i], p);
                count += CountDirection(r, c, -dr[i], -dc[i], p);
                if (count >= 5) return true;
            }
            return false;
        }

        int CountDirection(int r, int c, int dr, int dc, int p)
        {
            int cnt = 0, currR = r + dr, currC = c + dc;
            while (currR >= 0 && currR < 15 && currC >= 0 && currC < 15 && Board[currR, currC] == p)
            {
                cnt++; currR += dr; currC += dc;
            }
            return cnt;
        }
    }
}