using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server
{
	public void Launch()
	{
		ChatServer server = new ChatServer("127.0.0.1", 8000);
		server.Listen();
	}
}
