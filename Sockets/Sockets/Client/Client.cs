using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sockets.Client.Clients
{
    internal class Client
    {
        public string ID { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        Socket socket;
        public Client(Socket accepted)
        {
            socket = accepted;
            //Set id of user
            ID = Guid.NewGuid().ToString();
            EndPoint = (IPEndPoint)socket.RemoteEndPoint;
            socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
        }
        void callback(IAsyncResult result)
        {
            try
            {
                socket.EndReceive(result);

                byte[] buffer = new byte[8197];

                int rec = socket.Receive(buffer, buffer.Length, 0);

                if (rec < buffer.Length)
                {
                    Array.Resize(ref buffer, rec);
                }
                if (Received != null)
                {
                    Received(this, buffer);
                }
                socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                close();

                if (Disconnected != null)
                {
                    Disconnected(this);
                }
            }
        }
        public void close()
        {
            socket.Close();
            socket.Dispose();
        }

        public delegate void ClientReceivedHandler(Client sender, byte[] data);
        public delegate void ClientDisconnectedHandler(Client sender);
        public delegate void ClientConnectedHandler(Client sender);

        public event ClientReceivedHandler Received;
        public event ClientDisconnectedHandler Disconnected;
    }
}
