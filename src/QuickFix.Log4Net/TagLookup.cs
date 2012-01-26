namespace QuickFix.Log4Net
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class TagLookup
	{
		public static Dictionary<int, string> Tags;

		static TagLookup()
		{
			LoadTabDelimitedTags();
		}

		public static void LoadTabDelimitedTags()
		{
			using (var resource = typeof (TagLookup).Assembly.GetManifestResourceStream("QuickFix.Log4Net.FixTags.txt"))
			{
				LoadTabDelimitedTags(resource);
			}
		}

		public static void LoadTabDelimitedTags(Stream resource)
		{
			using (var file = new StreamReader(resource))
			{
				Tags = file.ReadToEnd()
					.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
					.Select(t => t.Split('\t'))
					.ToDictionary(t => Convert.ToInt32(t[0]), t => t[1]);
			}
		}

		public static string GetName(string id)
		{
			var idString = Convert.ToInt32(id);
			return GetName(idString);
		}

		public static string GetName(int id)
		{
			return Tags.ContainsKey(id) ? Tags[id] : "Invalid tag";
		}
	}
}