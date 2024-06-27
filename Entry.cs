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
	/// <summary>
	/// Storage for System.Net IPEndPoints as well as a basic database collection system for storing key-value pairs in the form of Strings.
	/// </summary>
	public struct Entry
	{
		/// <summary>
		/// A user's EndPoint when handling incoming and outgoing packets.
		/// </summary>
		public IPEndPoint remoteEndpoint = default;
		static IList<Listing> list = new List<Listing>();
		static string fileName;
		public Entry() { }
		/// <summary>
		/// Call this like with whatever file name for your database that you like. Required for this to work properly.
		/// </summary>
		/// <param name="fileNameNoExt">File name of the database (don't add a name extension).</param>
		public Entry(string fileNameNoExt)
		{
			fileName = fileNameNoExt + ".db";
			if (!Exists)
			{
				File.CreateText(fileName);
			}
		}
		/// <summary>
		/// Necessary for initializing the database.
		/// </summary>
		public static void Load()
		{
			LoadFromFile();
		}
		/// <summary>
		/// Getting the index based on the header value.
		/// </summary>
		/// <param name="header">Something like the name of the key/value pair.</param>
		/// <returns></returns>
		public int GetIndex(string header)
		{
			return list.IndexOf(
				   list.FirstOrDefault(t => t.header == header));
		}
		/// <summary>
		/// Adding a key/value pair entry--created a 'Listing' object and stores it.
		/// </summary>
		/// <param name="header">Something like the name of the key/value pair.</param>
		/// <param name="key">Typical key value required.</param>
		/// <param name="value">Any sort of string you'd like to include.</param>
		/// <returns></returns>
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
		/// <summary>
		/// Gets the 'Listing' object value based on the header.
		/// </summary>
		/// <param name="header">Something like the name of the key/value pair.</param>
		/// <returns>A 'Listing' object which holds specific data for the key/value pair, like whoAmI index.</returns>
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
		/// <summary>
		/// Gets the 'Listing' object value based on the whoAmI index. This is a one-based indexing.
		/// </summary>
		/// <param name="whoAmI">The specific index in question. While it begins the check from indexing of zero, the entries will be checked based on a one-based whoAmI identifier. 1 then would be the first 'Listing' database entry (array index: 0).</param>
		/// <returns>A 'Listing' object which holds specific data for the key/value pair, like whoAmI index.</returns>
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
		/// <summary>
		/// Gets the 'Listing' object value based on the whoAmI index. This is a one-based indexing.
		/// </summary>
		/// <param name="header">Header name, such as whatever the key/value pair is named.</param>
		/// <param name="whoAmI">The specific index in question. While it begins the check from indexing of zero, the entries will be checked based on a one-based whoAmI identifier. 1 then would be the first 'Listing' database entry (array index: 0).</param>
		/// <returns>A 'Listing' object which holds specific data for the key/value pair, like whoAmI index.</returns>
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
		/// <summary>
		/// Compares the two EndPoint values of 'Entry' objects.
		/// </summary>
		public static bool operator ==(Entry a, Entry b)
		{
			return a.remoteEndpoint == b.remoteEndpoint;
		}
		/// <summary>
		/// Compares the two EndPoint values of 'Entry' objects.
		/// </summary>
		public static bool operator !=(Entry a, Entry b)
		{
			return a.remoteEndpoint != b.remoteEndpoint;
		}
	}

	/// <summary>
	/// A database-type object that contains values for storing key/value pairs with names for each -- 'header' -- and a whoAmI index.
	/// </summary>
	public struct Listing
	{
		/// <summary>
		/// Index.
		/// </summary>
		public int whoAmI;
		/// <summary>
		/// Name of this object, or name of the key/value pair if you prefer.
		/// </summary>
		public string header;
		/// <summary>
		/// Key of course.
		/// </summary>
		public string key;
		/// <summary>
		/// Value of course.
		/// </summary>
		public string value;
		/// <summary>
		/// Initialize the object.
		/// </summary>
		/// <param name="header">Name of this object, or name of the key/value pair if you prefer.</param>
		/// <param name="key">Key of course.</param>
		/// <param name="value">Value of course.</param>
		/// <param name="whoAmI">Index.</param>
		public Listing(string header, string key, string value, int whoAmI)
		{
			this.whoAmI = whoAmI;
			this.header = header;
			this.key = key;
			this.value = value;
		}
		/// <summary>
		/// Converts a ToString() 'Listing' object representation to its literal form. Basically when this object is cast to string, it has extra values included. This method takes this string and puts it into a nice and neat object.
		/// </summary>
		/// <param name="value">The 'nice and neat' Listing object from its not-so-nice string representation.</param>
		/// <returns></returns>
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
		/// <summary>
		/// Compares the two 'Listing' objects by header only.
		/// </summary>
		public static bool operator ==(Listing a, Listing b)
		{
			return a.header == b.header;
		}
		/// <summary>
		/// Compares the two 'Listing' objects by header only.
		/// </summary>
		public static bool operator !=(Listing a, Listing b)
		{
			return a.header != b.header;
		}
	}
}
