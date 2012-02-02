namespace QuickFix.Log4Net
{
	using System;
	using System.Linq;

	public class RichMessageFormatter
	{
		private readonly DataDictionaryLookup _DataDictionaryLookup;

		public RichMessageFormatter(SessionID sessionId)
		{
			_DataDictionaryLookup = DataDictionaryLookup.InstanceBySessionProvider(sessionId);
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
					return GetTagValueTranslated(tagId, value);
			}
		}

		private string FormatMsgTypeValue(string msgType)
		{
			var name = _DataDictionaryLookup.GetMessageName(msgType);
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
			var tag = _DataDictionaryLookup.GetField(tagId);
			return tag == null
			       	? "Undefined tag"
			       	: tag.Name;
		}

		private string GetTagValueTranslated(int tagId, string value)
		{
			var tag = _DataDictionaryLookup.GetField(tagId);
			if (tag == null)
			{
				return value;
			}
			var description = tag.GetValueDescription(value);
			return string.IsNullOrWhiteSpace(description)
			       	? value
			       	: description + "[" + value + "]";
		}
	}
}