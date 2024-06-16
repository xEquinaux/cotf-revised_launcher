namespace cotf_revised
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Input s or c for server or client respectively.");
			string text = "";
			while (text != "s" || text != "c" || text != "exit");
			switch (text)
			{
				case "s":
					new Server().Launch();
					break;
				case "c":
					new Server().Launch();
					break;
				default:
				case "exit":
					break;
			}
		}
	}
}
