namespace Tests
{
	using NUnit.Framework;
	using QuickFix;
	using QuickFix.Log4Net;

	[TestFixture]
	public class RichMessageFormaterTests : AssertionHelper
	{
		[Test]
		public void Format_SingleTag_FormatsName()
		{
			var message = "9=8";
			var formatter = GetRichMessageFormatter();

			var formatted = formatter.Format(message);

			Expect(formatted, Is.EqualTo("BodyLength[9]=8"));
		}

		[Test]
		public void Format_TwoTags_Formats()
		{
			var message = "1=A" + RichMessageFormatter.TagSeparator + "9=8";
			var formatter = GetRichMessageFormatter();

			var formatted = formatter.Format(message);

			Expect(formatted, Is.EqualTo("Account[1]=A" + RichMessageFormatter.TagSeparator + "BodyLength[9]=8"));
		}

		[Test]
		public void Format_InvalidTagId_FormatsAsInvalidTag()
		{
			var message = "20003=A";
			var formatter = GetRichMessageFormatter();

			var formatted = formatter.Format(message);

			Expect(formatted, Is.EqualTo("Undefined tag[20003]=A"));
		}

		[Test]
		public void Format_InvalidTags_SeparatesAndShowsOriginal()
		{
			var message = "1=A" + RichMessageFormatter.TagSeparator + "garbage" + RichMessageFormatter.TagSeparator + "more=gar=bage";
			var formatter = GetRichMessageFormatter();

			var formatted = formatter.Format(message);

			Expect(formatted, Is.EqualTo("Account[1]=A" + RichMessageFormatter.TagSeparator + "invalid tag format: garbage" + RichMessageFormatter.TagSeparator + "more=gar=bage"));
		}

		[Test]
		public void Format_WithMsgType_LooksupMessageTypeName()
		{
			var message = "35=0";
			var formatter = GetRichMessageFormatter();

			var formatted = formatter.Format(message);

			Expect(formatted, Is.EqualTo("MsgType[35]=Heartbeat[0]"));
		}

		[Test]
		public void Format_WithInvalidMsgType_JustShowsNumber()
		{
			var message = "35=10000";
			var formatter = GetRichMessageFormatter();

			var formatted = formatter.Format(message);

			Expect(formatted, Is.EqualTo("MsgType[35]=10000"));
		}

		[Test]
		public void Format_WithFieldValues_ShowsFieldValue()
		{
			var message = "201=0";
			var formatter = GetRichMessageFormatter();

			var formatted = formatter.Format(message);

			Expect(formatted, Is.EqualTo("PutOrCall[201]=PUT[0]"));
		}

		private const string Fix42DataDictionary = @"..\spec\fix\FIX42.xml";

		private static RichMessageFormatter GetRichMessageFormatter()
		{
			DataDictionaryLookup.Default.Load(Fix42DataDictionary);
			return new RichMessageFormatter(default(SessionID));
		}
	}
}