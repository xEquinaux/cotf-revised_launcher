using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkMan
{
	public class Server
	{
		public IList<Entry> _client = new List<Entry>();

		public virtual void RegisterHooks()
		{
		}
		public byte[] DefaultIntercept(Packet packet)
		{
			return Packet.ConstructPacketData(1, packet.GetMessage());
		}

		public virtual void HandleMessage(Packet packet, Entry e, UdpClient udpServer)
		{
		}

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
				if (!_client.Contains(e))
				{
					_client.Add(e);
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