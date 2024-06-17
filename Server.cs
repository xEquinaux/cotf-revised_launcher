using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server
{
    private IList<User> _client = new List<User>();

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

            if (_client.FirstOrDefault(t => t.remoteEndpoint == remoteEP) == default)
            {
                User user = new User();
                user.remoteEndpoint = remoteEP;
                user.whoAmI
                _client.Add(remoteEP);
            }

            Packet packet = new Packet();

            packet.Id       = BitConverter.ToInt32(data, 0);
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
