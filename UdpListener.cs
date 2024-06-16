using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpListener
{
    public int whoAmI { get; set; } 
    private UdpClient udpClient;
    private IPEndPoint endPoint;

    public UdpListener(int port)
    {
        udpClient = new UdpClient(port);
        endPoint = new IPEndPoint(IPAddress.Any, port);
    }

    public void StartListening()
    {
        try
        {
            while (true)
            {
                byte[] data = udpClient.Receive(ref endPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"Received message: {message}");
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"SocketException: {ex}");
        }
        finally
        {
            udpClient.Close();
        }
    }
}
