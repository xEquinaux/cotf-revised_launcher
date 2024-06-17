using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpListener
{
    private UdpClient        udpClient;
    private IPEndPoint       endPoint;
    private BinaryReader     read;
    public static IList<Socket>    _client = new List<Socket>();

    public UdpListener(int port)
    {
        udpClient = new UdpClient (port);
        endPoint  = new IPEndPoint(IPAddress.Any, port);
    }

    public void StartListening(bool printMessages)
    {
        while (true)
        {
            try
            { 
                byte[] data = udpClient.Receive(ref endPoint);
                if (_client.Contains(udpClient.Client))
                {
                    _client.Add(udpClient.Client);
                }
                PacketManager.Instance.ReceivePacket(udpClient);
                if (printMessages)
                {
                    string message = Encoding.UTF8.GetString(data);
                    Console.WriteLine($"Received message: {message}");
                }
            }
            catch 
            {    
                if (printMessages)
                {
                    _client.Remove(udpClient.Client);
                    Console.WriteLine($"Client disconnected.");
                }
                continue; 
            }
        }
    }
}
