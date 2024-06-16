using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpListener
{
    private UdpClient     udpClient;
    private IPEndPoint    endPoint;
    private BinaryReader  read;
    private IList<Socket> _client = new List<Socket>();

    public UdpListener(int port)
    {
        udpClient = new UdpClient (port);
        endPoint  = new IPEndPoint(IPAddress.Any, port);
    }

    public void StartListening(bool printMessages)
    {
        try
        {
            while (true)
            {
                byte[] data = udpClient.Receive(ref endPoint);
                if (printMessages)
                {
                    string message = Encoding.UTF8.GetString(data);
                    Console.WriteLine($"Received message: {message}");
                }
            }
        }
        catch (SocketException ex)
        {
            if (printMessages)
            Console.WriteLine($"SocketException: {ex}");
        }
        finally
        {
            udpClient.Close();
        }
    }
}
