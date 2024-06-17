using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public class PacketHandler
{
	public PacketHandler()
	{
		PacketManager.Instance.RegisterPacketHandler((int)PacketId.Message, SendText);
	}
	public void SendText(Packet packet)
	{
		Console.WriteLine(packet);
	}
}
