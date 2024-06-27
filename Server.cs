using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkMan
{
	/// <summary>
	/// The networking management for the UDP-based server is located here, such as the HandleMessage hook and various events.
	/// </summary>
	public class Server
	{
		/// <summary>
		/// Contains all unique entries that have connected. Override 'DontStoreEntries' to false in order to begun default entry-storing. Otherwise handle this is 'HandleMessage' hook.
		/// </summary>
		public IList<Entry> Client 
		{
			get => _client;
			set => _client = value;
		}
		IList<Entry> _client = new List<Entry>();

		/// <summary>
		/// As you can imagine, this value manages default-entry storage in the 'Client' object. It defaults to true, as in 'Don't store.'
		/// </summary>
		public virtual bool DontStoreEntries => true;

		/// <summary>
		/// This is called before the Start(int port) method loop is begun.
		/// </summary>
		public virtual void RegisterHooks()
		{
		}

		byte[] DefaultIntercept(Packet packet)
		{
			return Packet.ConstructPacketData(1, packet.GetMessage());
		}

		/// <summary>
		/// Override this to work with the incoming and outgoing data.
		/// </summary>
		/// <param name="packet">Data and the like.</param>
		/// <param name="e">Contains data such as the IPEndPoint and various database-related variables.</param>
		/// <param name="udpServer">The UdpClient that the connection is stemmed from.</param>
		public virtual void HandleMessage(Packet packet, Entry e, UdpClient udpServer)
		{
		}

		/// <summary>
		/// This is basically the only method that needs to be run from a constructor to get it going. It simply begins the listening and connection management. Required.
		/// </summary>
		/// <param name="port">Port of whatever you want the internal UdpClient to be bound to. External clients will connect to this with their own UdpClients I suppose.</param>
		public void Start(int port)
		{
			new Entry("register");
			RegisterHooks();
			UdpClient udpServer = new UdpClient(port);

			while (true)
			{
				IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = udpServer.Receive(ref remoteEP);
				string message = Encoding.ASCII.GetString(data);

				if (message.ToLower() == "exit")
				{
					ExitEvent.Invoke();
					break;
				}
				var e = new Entry();
				e.remoteEndpoint = remoteEP;
				if (!DontStoreEntries)
				{
					if (!_client.Contains(e))
					{
						_client.Add(e);
					}
				}

				Packet packet = new Packet();
				packet.Id = BitConverter.ToInt32(data, 0);
				if (data.Length > 4)
				packet.ToWhom = BitConverter.ToInt32(data, 4);	 // <-- Server would be -1 for a global message, otherwise would be segmented
				if (data.Length > 8)
				packet.FromWhom = BitConverter.ToInt32(data, 8); // <-- Need to register whoAmI's to Entry objects, then get the index from the database
				if (data.Length > 12)
				packet.Data = data.Skip(12).ToArray();

				InterceptDataEvent?.Invoke(packet, e);

				HandleMessage(packet, e, udpServer);
			}

			udpServer.Close();
		}
		public static event InterceptMessage InterceptDataEvent;
		public static event Exit ExitEvent;
		public delegate void InterceptMessage(Packet packet, Entry e);
		public delegate void Exit();
	}
}