using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkMan
{
	public class PacketManager
	{
		//  Advanced packet management currently supplied as a class for expansion
		public static PacketManager Instance { get; set; }
		private Dictionary<int, Action<Packet>> packetHandlers;

		public PacketManager()
		{
			Instance = this;
			packetHandlers = new Dictionary<int, Action<Packet>>();
		}

		public void RegisterPacketHandler(int packetId, Action<Packet> handler)
		{
			if (!packetHandlers.ContainsKey(packetId))
			{
				packetHandlers.Add(packetId, handler);
			}
			else
			{
				throw new Exception("Packet handler for this ID already exists.");
			}
		}

		public void UnregisterPacketHandler(int packetId)
		{
			if (packetHandlers.ContainsKey(packetId))
			{
				packetHandlers.Remove(packetId);
			}
			else
			{
				throw new Exception("No packet handler found for this ID.");
			}
		}

		public void HandleIncomingPacket(Packet packet)
		{
			if (packetHandlers.ContainsKey(packet.Id))
			{
				packetHandlers[packet.Id].Invoke(packet);
			}
			else
			{
				throw new Exception("No packet handler found for this ID.");
			}
		}
	}
}