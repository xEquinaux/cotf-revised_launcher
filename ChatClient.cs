using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkMan
{
	public class ChatClient
	{
		public virtual void RegisterHooks()
		{
		}

		public void StartChat(string ip, int port)
		{
			RegisterHooks();
			UdpClient udpClient = new UdpClient();
			udpClient.Connect(ip, port);
			Console.WriteLine("Connected to server on " + ip + ":" + port);

			new Thread(() =>
			{ 
				while (true)
				{
					SendEvent?.Invoke(udpClient);
					HandleResponse(udpClient);
					ReceiveEvent?.Invoke(udpClient);
				}
			}).Start();
		}
		public virtual void HandleResponse(UdpClient client)
		{
		}
		public static event Send SendEvent;
		public static event Receive ReceiveEvent;
		public static event GeneratePacket GeneratePacketEvent;
		public static event ReturnOutput ReturnOutputEvent;
		public delegate void Send(UdpClient client);
		public delegate void Receive(UdpClient client);
		public delegate void ReturnOutput(Packet packet);
		public delegate byte[] GeneratePacket();
	}
}