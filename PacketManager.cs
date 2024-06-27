using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkMan
{
	/// <summary>
	/// Advanced packet management currently supplied as a class for expansion.
	/// </summary>
	public partial class PacketManager
	{
		/// <summary>
		/// The primary instance.
		/// </summary>
		public static PacketManager Instance { get; set; }
		private Dictionary<int, Action<Packet, UdpClient>> packetHandlers;

		/// <summary>
		/// This must be used once, preferably, per project because the 'Instance' variable is initialized here as well as the dictionary.
		/// </summary>
		public PacketManager()
		{
			Instance = this;
			packetHandlers = new Dictionary<int, Action<Packet, UdpClient>>();
		}

		/// <summary>
		/// Subscribe a Packet identity to a method for handling.
		/// </summary>
		/// <param name="packetId">Packet identity.</param>
		/// <param name="handler">Method representing what is handled upon packet ingest.</param>
		/// <exception cref="Exception"></exception>
		public void RegisterPacketHandler(int packetId, Action<Packet, UdpClient> handler)
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

		/// <summary>
		/// Unsubscribe a packet identity from its method.
		/// </summary>
		/// <param name="packetId">Packet identity.</param>
		/// <exception cref="Exception"></exception>
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

		/// <summary>
		/// Handles subscribed packets with their corresponding methods.
		/// </summary>
		/// <param name="packet">Incoming packet of data, typed 'Packet.'</param>
		/// <param name="u">The .NET UdpClient that was used to connect from the client to the server--that was listening for connections.</param>
		/// <exception cref="Exception"></exception>
		public void HandleIncomingPacket(Packet packet, UdpClient u)
		{
			if (packetHandlers.ContainsKey(packet.Id))
			{
				packetHandlers[packet.Id].Invoke(packet, u);
			}
			else
			{
				throw new Exception("No packet handler found for this ID.");
			}
		}
	}
}