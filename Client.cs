using System.Net.Sockets;

public class Client
{
	public Socket socket { get; set; }
	public void Launch()
	{
		ChatClient client = new ChatClient("127.0.0.1", 8000);
		socket = client.Client.Client;
		client.StartChat();
	}
}
