using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CirclePrefect.Dotnet;

public class User
{
	public static DataStore db;
	public string Passkey
	{
		get { return passkey.Substring(0, 4); }
		set { passkey = value; }
	}
	private string passkey;
	public string username;
	public int whoAmI;
	public IPEndPoint remoteEndpoint;
	public void WriteToDb()
	{

	}
}
