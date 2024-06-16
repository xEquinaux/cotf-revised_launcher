using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ChatClient
{
	public int whoAmI { get; set; }
	private UdpClient _client;
	private NetworkStream _stream;
	private UdpListener _listener;

	public ChatClient(string ip, int port)
	{
		_client = (_listener = new UdpListener(8000)).StartListening(false);
		Console.WriteLine("Connected to server on " + ip + ":" + port);
	}

	public void StartChat()
	{
		new Thread(() => ReceiveMessages()).Start();
		new Thread(() => ReceiveData()).Start();

		while (true)
		{
			string message = Console.ReadLine();
			byte[] buffer = Encoding.UTF8.GetBytes(message);
			_stream.Write(buffer, 0, buffer.Length);
		}
	}

	private void ReceiveData()
	{
		_listener.StartListening();
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
