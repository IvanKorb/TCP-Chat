using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class ClientSettings
    {
        readonly Socket s;
        public delegate void ReceivedEventHandler(ClientSettings cs, string received);
        public event ReceivedEventHandler Received = delegate { };
        public event EventHandler Connected = delegate { };
        public delegate void DisconnectedEventHandler(ClientSettings cs);
        public event DisconnectedEventHandler Disconnected = delegate {};
        bool connected;

        public ClientSettings()
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ip, int port)
        {
            try
            {
                var ep = new IPEndPoint(IPAddress.Parse(ip), port);
                s.BeginConnect(ep, ConnectCallback, s);
            }
            catch { }
        }

        public void Close()
        {
            s.Dispose();
            s.Close();
        }

        void ConnectCallback(IAsyncResult ar)
        {
            s.EndConnect(ar);
            connected = true;
            Connected(this, EventArgs.Empty);
            var buffer = new byte[s.ReceiveBufferSize];
            s.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReadCallback, buffer);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            var buffer = (byte[]) ar.AsyncState;
            var rec = s.EndReceive(ar);
            if (rec != 0)
            {
                var data = Encoding.UTF8.GetString(buffer, 0, rec);
                Received(this, data);
            }
            else
            {
                Disconnected(this);
                connected = false;
                Close();
                return;
            }
            s.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReadCallback, buffer);
        }

        public void Send(string data)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(data);
                s.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, buffer);
            }
            catch { Disconnected(this); }
        }

        void SendCallback(IAsyncResult ar)
        {
            s.EndSend(ar);
        }
    }
}