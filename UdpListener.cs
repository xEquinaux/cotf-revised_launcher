using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpListener
{
    private UdpClient udpClient;
    private IPEndPoint endPoint;

    public UdpListener(int port)
    {
        udpClient = new UdpClient(0);
        endPoint = new IPEndPoint(IPAddress.Any, 0);
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

    public UdpClient StartListening(bool displayErrors)
    {
        try
        {
            while (true)
            {
                udpClient.Receive(ref endPoint);
                return udpClient;
            }
        }
        catch (SocketException ex)
        {
            if (displayErrors)
            Console.WriteLine($"SocketException: {ex}");
        }
        finally
        {
            udpClient.Close();
        }
        return null;
    }
}
