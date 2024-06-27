using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkMan
{
	/// <summary>
	/// The primary means of storing, loading and processing network messages in this. It has a header of 12 bytes for the ID (Int32), ToWhom (Int32), and FromWhom (Int32), then all the data appended to the end of this is the byte data you might expect.
	/// </summary>
	public partial class Packet
	{
		/// <summary>
		/// The whom this packet is going to be sent to.
		/// </summary>
		public int ToWhom { get; set; }
		/// <summary>
		/// From whom where did this parket hark.
		/// </summary>
		public int FromWhom { get; set; }
		/// <summary>
		/// The packet value, or ID rather. Useful for switch statements.
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// All the data appended to the end of the packet (basically the chunk at index 12 of a raw incoming byte array).
		/// </summary>
		public byte[] Data { get; set; }

		public Packet()
		{
		}

		/// <summary>
		/// Create a 'Packet' object from raw data. Be sure to have the 12 byte chunk at the beginning of this, or simply insert blank values until index 12. Possible to create packets out of only the ID (first 4 bytes) and/or ToWhom and FromWhom missing. Can also neglect the data chunk appended at the end.
		/// </summary>
		/// <param name="data">The partial or complete header chunk and possibly the data chunk.</param>
		public Packet(byte[] data)
		{
			if (data.Length > 0)
			Id = BitConverter.ToInt32(data, 0);
			if (data.Length > 4)
			ToWhom = BitConverter.ToInt32(data, 4);
			if (data.Length > 8)
			FromWhom = BitConverter.ToInt32(data, 8);
			if (data.Length > 12)
			Data = data.Skip(12).ToArray();
		}

		/// <summary>
		/// Converts the entire packet into a byte array.
		/// </summary>
		/// <returns>The byte array.</returns>
		public byte[] GetBytes()
		{
			byte[] buffer = new byte[] { };
			buffer.Concat(BitConverter.GetBytes(Id))
				  .Concat(BitConverter.GetBytes(ToWhom))
				  .Concat(BitConverter.GetBytes(FromWhom))
				  .Concat(Data);
			return buffer;
		}
		
		/// <summary>
		/// Translates the Data chunk into a string.
		/// </summary>
		/// <returns>String representation of data.</returns>
		public string GetMessage()
		{
			if (Data != null && Data.Length >= 1)
			return Encoding.UTF8.GetString(Data);
			else return string.Empty;
		}

		/// <summary>
		/// Virtual method for converting an entry header and message into a byte array.
		/// </summary>
		/// <param name="e">Entry object.</param>
		/// <returns>A oddity of sorts containing the data to attempt a nice and proper chat message-type response.</returns>
		public virtual byte[] UsernameMessageBytes(Entry e)
		{
			return Encoding.UTF8.GetBytes(e.GetEntry(FromWhom).header + ": " + GetMessage());
		}

		/// <summary>
		/// Virtual method for converting the output of the ``GetMessage()`` method into an array. Usually trivial because the Data variable 'might' contain what you need.
		/// </summary>
		/// <returns></returns>
		public virtual byte[] MessageIntoBytes()
		{
			return Encoding.UTF8.GetBytes(GetMessage());
		}

		/// <summary>
		/// Helper method for constructing a packet out of variable data. Constructed in the order that the parameters are in (message will be last in the byte array: the data chunk).
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="toWhom">From whom where did this parket hark.</param>
		/// <param name="fromWhom">The packet value, or ID rather. Useful for switch statements.</param>
		/// <param name="message">All the data appended to the end of the packet (basically the chunk at index 12 of a raw incoming byte array).</param>
		/// <returns></returns>
		public static byte[] ConstructPacketData(int id, int toWhom = 255, int fromWhom = 0, string message = "")
		{
			byte[] _id = BitConverter.GetBytes(id);
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