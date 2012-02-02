namespace QuickFix.Log4Net
{
	using System;
	using System.Linq;
	using DataDictionary;

	public class RichMessageFormatter
	{
		public readonly DataDictionary SessionDataDictionary;
		private readonly DataDictionaryLookup _DataDictionaryLookup;

		public RichMessageFormatter(DataDictionary sessionDataDictionary)
		{
			SessionDataDictionary = sessionDataDictionary;
			_DataDictionaryLookup = DataDictionaryLookup.Instance;
		}

		public const char TagSeparator = '\x01';
		public const char KeyValueSeparator = '=';

		public string Format(string message)
		{
			var tags = message.Split(TagSeparator)
				.Where(t => !string.IsNullOrWhiteSpace(t))
				.Select(t => new {Original = t, Split = t.Split(KeyValueSeparator)});
			var validTags = tags.Where(t => t.Split.Count() == 2);
			var invalidTags = tags.Where(t => t.Split.Count() != 2);

			var formattedTags = validTags.Select(t => FormatTag(Convert.ToInt32(t.Split[0]), t.Split[1]));
			var formatted = String.Join(TagSeparator.ToString(), formattedTags);

			if (invalidTags.Any())
			{
				var invalid = TagSeparator + "invalid tag format: " + String.Join(TagSeparator.ToString(), invalidTags.Select(t => t.Original));
				formatted += invalid;
			}
			return formatted;
		}

		private string FormatTag(int tagId, string value)
		{
			return FormatTagId(tagId) + "=" + FormatTagValue(tagId, value);
		}

		private string FormatTagValue(int tagId, string value)
		{
			switch (tagId)
			{
				case 35:
					return FormatMsgTypeValue(value);
				default:
					return value;
			}
		}

		private string FormatMsgTypeValue(string msgType)
		{
			var name = _DataDictionaryLookup.GetName(msgType);
			if (string.IsNullOrWhiteSpace(name))
			{
				return msgType;
			}
			return name + "[" + msgType + "]";
		}

		private string FormatTagId(int tagId)
		{
			return GetTagName(tagId) + "[" + tagId + "]";
		}

		private string GetTagName(int tagId)
		{
			var tag = GetTag(tagId);
			return tag == null
			       	? "Undefined tag"
			       	: tag.Name;
		}

		private DDField GetTag(int tagId)
		{
			if (!SessionDataDictionary.FieldsByTag.ContainsKey(tagId))
			{
				return null;
			}
			return SessionDataDictionary.FieldsByTag[tagId];
		}
	}
}