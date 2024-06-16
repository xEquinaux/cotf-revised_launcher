
using System.Net.Sockets;
using System.Net;
using System.Text;

public class ChatServer
{
	public Dictionary<UdpClient, NetworkStream> Client => _clients;
	private UdpListener _server;
	private Dictionary<UdpClient, NetworkStream> _clients = new Dictionary<UdpClient, NetworkStream>();

	public ChatServer(string ip, int port)
	{
		_server = new UdpListener(8000);
		Console.WriteLine("Server started on " + ip + ":" + port);
	}

	public void Listen()
	{
		while (true)
		{
			UdpClient client = _server.StartListening(false);
			if (client != null)
			{ 
				NetworkStream stream = new NetworkStream(client.Client);
				_clients.Add(client, stream);

				Thread thread = new Thread(() => HandleClient(client, stream));
				thread.Start();
			}
		}
	}

	private void HandleClient(UdpClient client, NetworkStream stream)
	{
		byte[] buffer = new byte[1024];
		while (true)
		{
			int bytesRead = 0;
			try
			{
				bytesRead = stream.Read(buffer, 0, buffer.Length);
			}
			catch { Console.WriteLine($"Client disconnected."); return; }
			if (bytesRead == 0)
			{
				_clients.Remove(client);
				break;
			}

			string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
			BroadcastMessage(message);
		}
	}

	private void BroadcastMessage(string message)
	{
		byte[] buffer = Encoding.UTF8.GetBytes(message);
		foreach (var client in _clients.Values)
		{
			client.Write(buffer, 0, buffer.Length);
		}
	}
}

