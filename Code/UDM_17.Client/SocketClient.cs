using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UDM_17.Core;

namespace UDM_17.Client
{
    public class SocketClient
    {
        private TcpClient? _client;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        public event Action<Packet>? PacketReceived;
        public event Action? Disconnected;

        public bool IsConnected => _client?.Connected == true;

        public async Task ConnectAsync(string host = "127.0.0.1", int port = 1234)
        {
            if (IsConnected)
            {
                return;
            }

            _client = new TcpClient();
            await _client.ConnectAsync(host, port);
            var stream = _client.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            _ = Task.Run(ReceiveLoop);
        }

        public async Task SendAsync(Packet packet)
        {
            if (_writer == null)
            {
                return;
            }

            await _writer.WriteLineAsync(packet.ToJson());
        }

        public async Task DisconnectAsync()
        {
            try
            {
                if (_writer != null)
                {
                    await _writer.FlushAsync();
                }
            }
            catch
            {
            }

            _reader?.Dispose();
            _writer?.Dispose();
            _client?.Close();
            _reader = null;
            _writer = null;
            _client = null;
        }

        private async Task ReceiveLoop()
        {
            if (_reader == null)
            {
                return;
            }

            try
            {
                while (true)
                {
                    string? json = await _reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        break;
                    }

                    Packet? packet = Packet.FromJson(json);
                    if (packet != null)
                    {
                        PacketReceived?.Invoke(packet);
                    }
                }
            }
            catch
            {
            }

            Disconnected?.Invoke();
        }
    }
}
