using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMan
{
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
