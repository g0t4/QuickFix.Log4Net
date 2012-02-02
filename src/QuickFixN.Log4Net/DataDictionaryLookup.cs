namespace QuickFix.Log4Net
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml;

	public class DataDictionaryLookup
	{
		public static Func<SessionID, DataDictionaryLookup> InstanceBySessionProvider = sessionID => Default;

		public static DataDictionaryLookup Default = new DataDictionaryLookup();

		public readonly Dictionary<string, Message> MessagesByType = new Dictionary<string, Message>();
		public readonly Dictionary<int, Field> FieldsByNumber = new Dictionary<int, Field>();

		static DataDictionaryLookup()
		{
		}

		public void Load(string path)
		{
			var xml = new XmlDocument();
			xml.Load(path);
			LoadMessages(xml);
			LoadFields(xml);
		}

		private void LoadFields(XmlNode xml)
		{
			xml.SelectNodes("//fields/field")
				.OfType<XmlNode>()
				.Select(n => new Field(n))
				.ToList()
				.ForEach(n => FieldsByNumber[n.Number] = n);
		}

		private void LoadMessages(XmlNode xml)
		{
			xml.SelectNodes("//messages/message")
				.OfType<XmlNode>()
				.Select(n => new Message(n))
				.ToList()
				.ForEach(n => MessagesByType[n.MessageType] = n);
		}

		public string GetMessageName(string msgType)
		{
			var message = GetMessage(msgType);
			return message != null
			       	? message.Name
			       	: null;
		}

		public Message GetMessage(string msgType)
		{
			return MessagesByType.ContainsKey(msgType)
			       	? MessagesByType[msgType]
			       	: null;
		}

		public string GetFieldName(int tagId)
		{
			var field = GetField(tagId);
			return field != null
			       	? field.Name
			       	: null;
		}

		public Field GetField(int tagId)
		{
			return FieldsByNumber.ContainsKey(tagId)
			       	? FieldsByNumber[tagId]
			       	: null;
		}

		public class Message
		{
			public Message(XmlNode node)
			{
				Name = node.Attributes["name"].Value;
				MessageType = node.Attributes["msgtype"].Value;
				Category = node.Attributes["msgcat"].Value;
			}

			public string Name { get; set; }
			public string MessageType { get; set; }
			public string Category { get; set; }
		}

		public class Field
		{
			public Field(XmlNode node)
			{
				Name = node.Attributes["name"].Value;
				Number = Convert.ToInt32(node.Attributes["number"].Value);
				Type = node.Attributes["type"].Value;

				ValuesByEnum = node.SelectNodes("value")
					.OfType<XmlNode>()
					.Select(v => new Value(v))
					.ToDictionary(v => v.Enum);
			}

			public class Value
			{
				public Value(XmlNode node)
				{
					Enum = node.Attributes["enum"].Value;
					Description = node.Attributes["description"].Value;
				}

				public string Enum { get; set; }
				public string Description { get; set; }
			}

			public string Name { get; set; }
			public int Number { get; set; }
			public string Type { get; set; }

			public IDictionary<string, Value> ValuesByEnum { get; set; }

			public string GetValueDescription(string value)
			{
				return ValuesByEnum.ContainsKey(value)
				       	? ValuesByEnum[value].Description
				       	: null;
			}
		}
	}
}