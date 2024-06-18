using System.Net.Sockets;

public class Client
{
	public void Launch()
	{
		ChatClient client = new ChatClient("127.0.0.1", 8000);
		client.StartChat();
	}
}
