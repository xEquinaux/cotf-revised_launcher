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
		/// <summary>
		/// Listen for a "SUCCESS" message.
		/// </summary>
		/// <param name="u">The UdpClient to use.</param>
		/// <param name="desiredResponse">The response you want to receive.</param>
		/// <returns>Did it receive a proper message.</returns>
		public static bool Retrieve(UdpClient u, string desiredResponse)
		{
			IPEndPoint end = new IPEndPoint(IPAddress.Any, 0);
			return Encoding.UTF8.GetString(u.Receive(ref end), 0, desiredResponse.Length) == desiredResponse;
		}
	}
}