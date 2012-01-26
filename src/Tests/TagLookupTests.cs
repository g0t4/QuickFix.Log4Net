namespace Tests
{
	using NUnit.Framework;
	using QuickFix;
	using QuickFix.Log4Net;

	public class TagLookupTests : AssertionHelper
	{
		[Test]
		public void LoadTags_FromResource_LoadsTagsAndHasMessageType()
		{
			var tags = TagLookup.Tags;

			var messageType = tags[MsgType.FIELD];

			Expect(messageType, Is.EqualTo("MsgType"));
		}
	}
}