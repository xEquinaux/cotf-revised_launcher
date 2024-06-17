using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ChatClient
{
	public virtual void RegisterHooks()  
	{
	}

	public void StartChat(string ip, int port)
	{
		RegisterHooks();
		UdpClient udpClient = new UdpClient();
        udpClient.Connect(ip, 8000);
		Console.WriteLine("Connected to server on " + ip + ":" + port);
		
		while (true)
		{
			byte[] buffer = new byte[] { };
			buffer = Packet.ConstructPacketData(1, Console.ReadLine());
			udpClient.Send(buffer, buffer.Length);
		
			IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

			Console.WriteLine("Received: " + new Packet(receiveBytes).GetMessage());
        }
	}

	public static event GeneratePacket GeneratePacketEvent;
	public static event ReturnOutput ReturnOutputEvent;
	public delegate void ReturnOutput(Packet packet);
	public delegate byte[] GeneratePacket(); 
}