
using System.Net.Sockets;
using System.Net;
using System.Text;

public class ChatServer
{
	private TcpListener _server;
	private Dictionary<TcpClient, NetworkStream> _clients = new Dictionary<TcpClient, NetworkStream>();

	public ChatServer(string ip, int port)
	{
		_server = new TcpListener(IPAddress.Parse(ip), port);
		_server.Start();
		Console.WriteLine("Server started on " + ip + ":" + port);
	}

	public void Listen()
	{
		while (true)
		{
			TcpClient client = _server.AcceptTcpClient();
			NetworkStream stream = client.GetStream();
			_clients.Add(client, stream);

			Thread thread = new Thread(() => HandleClient(client, stream));
			thread.Start();
		}
	}

	private void HandleClient(TcpClient client, NetworkStream stream)
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

