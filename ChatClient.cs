using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkMan
{
	public class ChatClient
	{
		public UdpClient Init(string ip, int port)
		{
			UdpClient udpClient = new UdpClient();
			udpClient.Connect(ip, port);
			Console.WriteLine("Connected to server on " + ip + ":" + port);
			return udpClient;
		}
	}
}