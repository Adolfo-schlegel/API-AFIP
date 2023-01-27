using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sockets.Server
{
	class SocketAcceptedEventHandler : EventArgs
	{
		public Socket Accepted { get; private set; }
		public SocketAcceptedEventHandler(Socket s)
		{
			Accepted = s;
		}
	}

	internal class Listener
	{
		public bool Listening { get; private set; }
		public int Port { get; private set; }

		Socket s;

		public delegate void SocketAcceptedHandler(Socket socket);
		public event EventHandler<SocketAcceptedEventHandler> SocketAccepted;
		public Listener(int port)
		{
			//Configure port server
			Port = port;
			s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public void Start()
		{
			if (Listening)
				return;

			s.Bind(new IPEndPoint(0, Port));
			s.Listen(0);

			s.BeginAccept(callback, null);
			Listening = true;
		}

		public void Stop()
		{
			if (!Listening)
				return;

			s.Close();
			s.Dispose();
			s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		void callback(IAsyncResult ar)
		{
			//esto va a tomar el socket que fue conectado
			try
			{
				Socket s = this.s.EndAccept(ar);

				if (SocketAccepted != null)
				{
					SocketAccepted(this, new SocketAcceptedEventHandler(s));
				}

				this.s.BeginAccept(callback, null);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
