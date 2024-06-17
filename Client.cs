using System.Net;
using System.Net.Sockets;

public class Client
{
	public void Launch(string ip)
	{
		ChatClient client = new ChatClient(ip, 8000);
		client.StartChat();
	}
}
