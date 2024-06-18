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

    public virtual void HandleMessage(UdpClient client)
    {
    }

	public void Start(int port)
    {
        RegisterHooks();
        UdpClient udpServer = new UdpClient(port); 

        while (true)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpServer.Receive(ref remoteEP);
            string message = Encoding.ASCII.GetString(data);

            if (message.ToLower() == "exit") // admin commands?
            {
                ExitEvent.Invoke();
                break;
            }
            
            Packet packet = new Packet();
            packet.Id       = BitConverter.ToInt32(data, 0);

            switch (packet.Id)
            {
                case (int)PacketId.None:
                case (int)PacketId.Success:
                case (int)PacketId.PingEveryone:
                case (int)PacketId.Message:
                    break;
                case (int)PacketId.Login:
                    break;
            }
            if (_client.FirstOrDefault(t => t.remoteEndpoint == remoteEP) == default(Entry))
            {
                Entry entry = new Entry("dictionary");
                entry   .remoteEndpoint = remoteEP;
                entry   .AddEntry()
                _client .Add(entry);
            }


            packet.ToWhom   = BitConverter.ToInt32(data, 4);
            packet.FromWhom = _client.IndexOf(remoteEP);
            packet.Data     = data.Skip(12).ToArray(); 

            byte[] response = DefaultIntercept(packet);
            foreach (var item in _client)
            {
                udpServer.Send(response, response.Length, item);
            }
        }

        udpServer.Close();
    }
    public static event InterceptMessage InterceptDataEvent;
    public static event Exit ExitEvent;
    public delegate byte[] InterceptMessage(Packet packet);
    public delegate void Exit();
}
