using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkMan
{
	/// <summary>
	/// Be sure to call Init from this class for a basic client setup.
	/// </summary>
	public partial class ChatClient
	{
		/// <summary>
		/// Basic initialization of a UdpClient object.
		/// </summary>
		/// <param name="ip">Domain name or address.</param>
		/// <param name="port">Numerical port number.</param>
		/// <returns>A .NET UdpClient from System.Net.Sockets.</returns>
		public UdpClient Init(string ip, int port)
		{
			UdpClient udpClient = new UdpClient();
			udpClient.Connect(ip, port);
			Console.WriteLine("Connected to server on " + ip + ":" + port);
			return udpClient;
		}
	}
}