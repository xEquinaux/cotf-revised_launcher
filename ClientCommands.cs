using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ClientCommands
{
	public ClientCommands()
	{
		new PacketManager().RegisterPacketHandler(0, (Packet p) => { });
	}
	public void SendRandomNumber(Packet p)
	{
		
	}
}
