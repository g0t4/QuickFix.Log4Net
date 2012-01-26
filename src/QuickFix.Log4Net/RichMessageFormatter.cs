namespace QuickFix.Log4Net
{
	using System;
	using System.Linq;

	public class RichMessageFormatter
	{
		public const char TagSeparator = '\x01';
		public const char KeyValueSeparator = '=';

		public string Format(string message)
		{
			var tags = message.Split(TagSeparator)
				.Where(t => !string.IsNullOrWhiteSpace(t))
				.Select(t => new {Original = t, Split = t.Split(KeyValueSeparator)});
			var validTags = tags.Where(t => t.Split.Count() == 2);
			var invalidTags = tags.Where(t => t.Split.Count() != 2);

			var formattedTags = validTags.Select(t => TagLookup.GetName(t.Split[0]) + "[" + t.Split[0] + "]=" + t.Split[1]);
			var formatted = String.Join(TagSeparator.ToString(), formattedTags);

			if (invalidTags.Any())
			{
				var invalid = TagSeparator + "invalid tag format: " + String.Join(TagSeparator.ToString(), invalidTags.Select(t => t.Original));
				formatted += invalid;
			}
			return formatted;
		}
	}
}