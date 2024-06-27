using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMan
{
	/// <summary>
	/// Already stamped packet identities. Since the methods take an Int32, this is just for reference.
	/// </summary>
	public enum PacketId : int
	{
		None = 0,
		Message = 1,
		PingEveryone = 10,
		Login = 128,
		Success = 220,
		Status = 4
	}
}
