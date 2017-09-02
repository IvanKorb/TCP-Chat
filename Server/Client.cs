using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Client
    {
        public delegate void ClientReceivedHandler(Client sender, byte[] data);
        public delegate void ClientDisconnectedHandler(Client sender);
        public event ClientReceivedHandler Received;
        public event ClientDisconnectedHandler Disconnected;

        public IPEndPoint Ip { get; private set; }

        public Socket socket;

        public Client(Socket accepted)
        {
            socket = accepted;
            Ip = (IPEndPoint) socket.RemoteEndPoint;
            socket.BeginReceive(new byte[] {0}, 0, 0, 0, Callback, null);
        }

        void Callback(IAsyncResult ar)
        {
            try
            {
                socket.EndReceive(ar);
                var buffer = new byte[socket.ReceiveBufferSize];
                var rec = socket.Receive(buffer, buffer.Length, 0);
                if (rec < buffer.Length)
                {
                    Array.Resize(ref buffer, rec);
                }
                Received?.Invoke(this, buffer);
                socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, Callback, null);
               
            }
            catch (Exception)
            {
                Close();
                Disconnected?.Invoke(this);
            }
        }

        public void Send(string data)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, ar => socket.EndSend(ar), buffer);
        }

        public void Close()
        {
            socket.Dispose();
            socket.Close();
        }
    }
}