public class Program
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
				new UdpListener(8000).StartListening(true);
				break;
			case "c":
				new Client().Launch();
				break;
			case "exit":
				Environment.Exit(0);
				break;
		}
	}
}
