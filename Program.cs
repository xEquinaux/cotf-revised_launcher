namespace cotf_revised
{
	internal class Program
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
					new Server().Launch();
					break;
				case "c":
					new Client().Launch();
					break;
				default:
				case "exit":
					break;
			}
		}
	}
}
