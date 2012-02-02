namespace QuickFix.Log4Net
{
	using System;
	using log4net;

	public class Log4NetLogBase
	{
		private readonly SessionID _SessionId;
		public ILog Log;
		public static ILog FormattingLog = LogManager.GetLogger("FormattingLog");

		public static string LoggerName = "QuickFix";
		public bool RichMessageLogging { get; set; }

		public Log4NetLogBase(SessionID sessionId)
		{
			_SessionId = sessionId;
			Log = LogManager.GetLogger(LoggerName + "-" + sessionId);
		}

		protected virtual string GetMessageParsed(string @string)
		{
			if (!RichMessageLogging)
			{
				return @string;
			}
			try
			{
				var session = Session.LookupSession(_SessionId);
				if (session == null)
				{
					FormattingLog.Error("No session found for session: " + _SessionId);
					return @string;
				}
				return new RichMessageFormatter(session.SessionDataDictionary).Format(@string);
			}
			catch (Exception exception)
			{
				FormattingLog.Error("Error formatting: " + @string, exception);
				return @string;
			}
		}
	}
}