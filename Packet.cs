using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkMan
{
	public class Packet
	{
		public int ToWhom { get; set; }
		public int FromWhom { get; set; }
		public int Id { get; set; }
		public byte[] Data { get; set; }

		public Packet()
		{
		}
		public Packet(byte[] data)
		{
			if (data.Length >= 4)
			Id = BitConverter.ToInt32(data, 0);
			if (data.Length >= 8)
			ToWhom = BitConverter.ToInt32(data, 4);
			if (data.Length >= 12)
			FromWhom = BitConverter.ToInt32(data, 8);
			if (data.Length >= 13)
			Data = data.Skip(12).ToArray();
		}
		public byte[] GetBytes()
		{
			byte[] buffer = new byte[] { };
			buffer.Concat(BitConverter.GetBytes(Id))
					 .Concat(BitConverter.GetBytes(ToWhom))
					 .Concat(BitConverter.GetBytes(FromWhom))
					 .Concat(Data);
			return buffer;
		}
		public string GetMessage()
		{
			if (Data != null && Data.Length >= 13)
			return Encoding.UTF8.GetString(Data);
			else return string.Empty;
		}

		public static byte[] ConstructPacketData(int id, string message, int toWhom = 255, int fromWhom = 0)
		{
			byte[] _id = BitConverter.GetBytes(1);
			if (message == null)
			{
				return _id;
			}
			byte[] _toWhom = BitConverter.GetBytes(toWhom);
			byte[] _fromWhom = BitConverter.GetBytes(fromWhom);
			byte[] _message = Encoding.UTF8.GetBytes(message);
			return _id.Concat(_toWhom).Concat(_fromWhom).Concat(_message).ToArray();
		}

		public override string ToString()
		{
			return $"Packet:{Id} To:{ToWhom} From:{FromWhom} Data_Len:{Data.Length}";
		}
	}
}