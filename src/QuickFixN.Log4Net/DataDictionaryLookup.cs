namespace QuickFix.Log4Net
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml;

	public class DataDictionaryLookup : Dictionary<string, string>
	{
		public static DataDictionaryLookup Instance = new DataDictionaryLookup();

		static DataDictionaryLookup()
		{
		}

		public void Load(string path)
		{
			var xml = new XmlDocument();
			xml.Load(path);
			xml.SelectNodes("//messages/message")
				.OfType<XmlNode>()
				.Select(n => new {Name = n.Attributes["name"].Value, MsgType = n.Attributes["msgtype"].Value})
				.ToList()
				.ForEach(n => this[n.MsgType] = n.Name);
		}

		public string GetName(string msgType)
		{
			return ContainsKey(msgType)
			       	? this[msgType]
			       	: null;
		}

		// todo enum options on fields, cuz of course quickfix/N didn't load this... common guys!  sounds like time to send them a pull request
	}
}