using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace co_caro
{
    public class ServerManager
    {
        private TcpListener server;
        private List<ClientHandler> waitingClients = new List<ClientHandler>();
        private List<GameRoom> rooms = new List<GameRoom>();

        public void Start(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            Console.WriteLine("Server started...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ClientHandler handler = new ClientHandler(client, this);

                Thread t = new Thread(handler.HandleClient);
                t.Start();
            }
        }

        public void AddClient(ClientHandler client)
        {
            lock (waitingClients)
            {
                waitingClients.Add(client);

                if (waitingClients.Count >= 2)
                {
                    var p1 = waitingClients[0];
                    var p2 = waitingClients[1];

                    waitingClients.RemoveAt(0);
                    waitingClients.RemoveAt(0);

                    GameRoom room = new GameRoom(p1, p2);
                    rooms.Add(room);

                    room.StartGame();
                }
            }
        }
    }

    // ================= CLIENT =================
    public class ClientHandler
    {
        private TcpClient client;
        private NetworkStream stream;
        private ServerManager server;

        public GameRoom Room { get; set; }
        public string Symbol { get; set; }

        public ClientHandler(TcpClient client, ServerManager server)
        {
            this.client = client;
            this.server = server;
            this.stream = client.GetStream();
        }

        public void HandleClient()
        {
            Console.WriteLine("Client connected!");
            server.AddClient(this);

            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);
                if (bytes == 0) break;

                string msg = Encoding.UTF8.GetString(buffer, 0, bytes);
                Console.WriteLine("Received: " + msg);

                Room?.HandleMove(this, msg);
            }
        }

        public void Send(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }
    }

    // ================= ROOM =================
    public class GameRoom
    {
        private ClientHandler p1, p2;
        private char[,] board = new char[15, 15];
        private ClientHandler currentTurn;

        public GameRoom(ClientHandler a, ClientHandler b)
        {
            p1 = a;
            p2 = b;

            a.Room = this;
            b.Room = this;

            a.Symbol = "X";
            b.Symbol = "O";

            currentTurn = a;
        }

        public void StartGame()
        {
            p1.Send("START:X");
            p2.Send("START:O");

            currentTurn.Send("YOUR_TURN");
        }

        public void HandleMove(ClientHandler player, string msg)
        {
            if (player != currentTurn) return;

            // MOVE:x,y
            string[] parts = msg.Split(':')[1].Split(',');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);

            board[x, y] = player.Symbol[0];

            Broadcast($"MOVE:{x},{y},{player.Symbol}");

            if (CheckWin(x, y))
            {
                Broadcast($"WIN:{player.Symbol}");
                return;
            }

            SwitchTurn();
        }

        private void SwitchTurn()
        {
            currentTurn = (currentTurn == p1) ? p2 : p1;
            currentTurn.Send("YOUR_TURN");
        }

        private void Broadcast(string msg)
        {
            p1.Send(msg);
            p2.Send(msg);
        }

        private bool CheckWin(int x, int y)
        {
            char s = board[x, y];
            int count;

            // ngang
            count = 0;
            for (int i = 0; i < 15; i++)
            {
                count = (board[x, i] == s) ? count + 1 : 0;
                if (count >= 5) return true;
            }

            // dọc
            count = 0;
            for (int i = 0; i < 15; i++)
            {
                count = (board[i, y] == s) ? count + 1 : 0;
                if (count >= 5) return true;
            }

            return false;
        }
    }
}
