using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NetworkMan
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Input s or c for server or client respectively.");
			string text = "";
			do
			{
				text = Console.ReadLine();
			} while (text != "s" && text != "c" && text != "exit");
			switch (text)
			{
				case "s":
					var s = new Server();
					s.Start(8000);
					break;
				case "c":
					Console.Write("Enter IP address for connection: ");
					var c = new ChatClient();
					c.Init(Console.ReadLine(), 8000);
					break;
				case "exit":
					Environment.Exit(0);
					break;
			}
		}
	}
}