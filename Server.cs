using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server
{
    private IList<Entry> _client = new List<Entry>();

    public virtual void RegisterHooks()
    {
	}
    public byte[] DefaultIntercept(Packet packet)
    {
        return Packet.ConstructPacketData(1, packet.GetMessage());
    }

    public virtual void HandleMessage(Packet packet, Entry e, ref UdpClient udpServer)
    {
    //  Example packet management
        switch (packet.Id)
        {
            case (int)PacketId.None:
            case (int)PacketId.Success:
            case (int)PacketId.PingEveryone:
            case (int)PacketId.Message:
                byte[] response = DefaultIntercept(packet);
                foreach (var item in _client)
                {
                    if (e != item)
                    {
                        udpServer.Send(response, response.Length, item.remoteEndpoint);
                    }
                }
                break;
            case (int)PacketId.Login:
            //  TODO: work login details into client
                if (e.GetEntry("DEFAULT") != default)
                {
                    e.AddEntry("DEFAULT", "DEFAULT", "DEFAULT");
                }
                break;
        }
    }

	public void Start(int port)
    {
        new Entry("register");
        RegisterHooks();
        UdpClient udpServer = new UdpClient(port); 

        while (true)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data         = udpServer.Receive(ref remoteEP);
            string message      = Encoding.ASCII.GetString(data);

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

            Packet packet   = new Packet();
            packet.Id       = BitConverter.ToInt32(data, 0);
            packet.ToWhom   = BitConverter.ToInt32(data, 4); // <-- Server would be -1 for a global message, otherwise would be segmented
            packet.FromWhom = BitConverter.ToInt32(data, 8); // <-- Need to register whoAmI's to Entry objects, then get the index from the database
            packet.Data     = data.Skip(12).ToArray();       

            InterceptDataEvent?.Invoke(packet, e);

            HandleMessage(packet, e, ref udpServer);
        }

        udpServer.Close();
    }
    public static event InterceptMessage InterceptDataEvent;
    public static event Exit ExitEvent;
    public delegate void InterceptMessage(Packet packet, Entry e);
    public delegate void Exit();
}
