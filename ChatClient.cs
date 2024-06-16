using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ChatClient
{
	private TcpClient _client;
	private NetworkStream _stream;

	public ChatClient(string ip, int port)
	{
		_client = new TcpClient(ip, port);
		_stream = _client.GetStream();
		Console.WriteLine("Connected to server on " + ip + ":" + port);
	}

	public void StartChat()
	{
		Thread thread = new Thread(() => ReceiveMessages());
		thread.Start();

		while (true)
		{
			string message = Console.ReadLine();
			byte[] buffer = Encoding.UTF8.GetBytes(message);
			_stream.Write(buffer, 0, buffer.Length);
		}
	}

	private void ReceiveMessages()
	{
		byte[] buffer = new byte[1024];
		while (true)
		{
			int bytesRead = _stream.Read(buffer, 0, buffer.Length);
			if (bytesRead == 0)
			{
				break;
			}

			string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
			Console.WriteLine("Received: " + message);
		}
	}
}
