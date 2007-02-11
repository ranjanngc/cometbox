using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

namespace cometbox.HTTP
{
    public class Server
    {
        private Thread thread = null;
		private static TcpListener listener = null;

		public Server(IPAddress ip, Int32 port)
		{
            Console.WriteLine("Server: Starting on {0}:{1}.", ip.ToString(), port);
			listener = new TcpListener(ip, port);
			listener.Start();
		
			thread = new Thread(new ThreadStart(Loop));
			thread.Start();
		}
				
		private void Loop()
		{
			List<Client> clients = new List<Client>();
	
			while ( true ) {
				TcpClient client = listener.AcceptTcpClient();

				Client dc = new Client(client, this);
				clients.Add(dc);
				
				int i = 0;
				while (i < clients.Count) {
					if ( !clients[i].IsLive ) {
						clients.RemoveAt(i);
					}
					i++;
				}
			}
		}
		
		public bool IsRunning()
		{
			return thread.IsAlive;
		}
    }
}