namespace Tests
{
	using NUnit.Framework;
	using QuickFix.Log4Net;

	[TestFixture]
	public class RichMessageFormaterTests : AssertionHelper
	{
		[Test]
		public void Format_ValidTag_FormatsName()
		{
			var message = "35=A";
			var formatter = new RichMessageFormatter();

			var Formatd = formatter.Format(message);

			Expect(Formatd, Is.EqualTo("MsgType[35]=A"));
		}

		[Test]
		public void Format_TwoTags_Formats()
		{
			var message = "35=A" + RichMessageFormatter.TagSeparator + "9=8";
			var Formatr = new RichMessageFormatter();

			var Formatd = Formatr.Format(message);

			Expect(Formatd, Is.EqualTo("MsgType[35]=A" + RichMessageFormatter.TagSeparator + "BodyLength[9]=8"));
		}

		[Test]
		public void Format_InvalidTags_SeparatesAndShowsOriginal()
		{
			var message = "35=A" + RichMessageFormatter.TagSeparator + "garbage" + RichMessageFormatter.TagSeparator + "more=gar=bage";
			var Formatr = new RichMessageFormatter();

			var Formatd = Formatr.Format(message);

			Expect(Formatd, Is.EqualTo("MsgType[35]=A" + RichMessageFormatter.TagSeparator + "invalid tag format: garbage" + RichMessageFormatter.TagSeparator + "more=gar=bage"));
		}
	}
}