using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMan
{ 
	public struct Entry
	{
		public IPEndPoint remoteEndpoint = default;
		static IList<Listing> list = new List<Listing>();
		static string fileName;
		public Entry() { }
		public Entry(string fileNameNoExt)
		{
			fileName = fileNameNoExt + ".db";
			if (!Exists)
			{
				File.CreateText(fileName);
			}
		}
		public static void Load()
		{
			LoadFromFile();
		}
		public int GetIndex(string header)
		{
			return list.IndexOf(
				   list.FirstOrDefault(t => t.header == header));
		}
		public Listing AddEntry(string header, string key, string value)
		{
			if (list.FirstOrDefault(t => t.header == header) == default)
			{
				var l  = new Listing(header, key, value, list.Count + 1);
				list   .Add(l);
				WriteToFile(l);
				return l;
			}
			return default;
		}
		public Listing GetEntry(string header)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].header == header)
				{
					return list[i];
				}
			}
			return default;
		}
		public Listing GetEntry(int whoAmI)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].whoAmI == whoAmI)
				{
					return list[i];
				}
			}
			return default;
		}
		public Listing GetEntry(string header, int whoAmI)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].header == header && i == whoAmI)
				{
					return list[i];
				}
			}
			return default;
		}
		static bool Exists => File.Exists(fileName);
		static void LoadFromFile()
		{		
			if (!Exists)
			{
				var f = File.CreateText(fileName);
				f.Close();
			}
			using (StreamReader sr = new StreamReader(fileName))
			{
				while (!sr.EndOfStream)
				{
					list.Add(Listing.Extract(sr.ReadLine().Replace(" : ", "_:_")));
				}
			}
		}
		void WriteToFile(Listing list)
		{
			using (StreamWriter sw = new StreamWriter(fileName, true))
			{
				sw.WriteLine(list.ToString().Replace("_:_", " : "));
			}
		}
		public static bool operator ==(Entry a, Entry b)
		{
			return a.remoteEndpoint == b.remoteEndpoint;
		}
		public static bool operator !=(Entry a, Entry b)
		{
			return a.remoteEndpoint != b.remoteEndpoint;
		}
	}

	public struct Listing
	{
		public int whoAmI;
		public string header;
		public string key;
		public string value;
		public Listing(string header, string key, string value, int whoAmI)
		{
			this.whoAmI = whoAmI;
			this.header = header;
			this.key = key;
			this.value = value;
		}
		public static Listing Extract(string value)
		{
			Listing list = new Listing();
			string[] split = value.Split("_:_", StringSplitOptions.RemoveEmptyEntries);
			int.TryParse(split[0], out list.whoAmI);
			list.header = split[1]
				.Replace("[", "")
				.Replace("]", "");
			list.key = split[2];
			list.value = split[3];
			return list;
		}
		public override string ToString()
		{
			return $"{whoAmI}_:_[{header}]_:_{key}_:_{value}";
		}
		public static bool operator ==(Listing a, Listing b)
		{
			return a.header == b.header;
		}
		public static bool operator !=(Listing a, Listing b)
		{
			return a.header != b.header;
		}
	}
}
