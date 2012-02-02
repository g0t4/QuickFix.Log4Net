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
				return new RichMessageFormatter(_SessionId).Format(@string);
			}
			catch (Exception exception)
			{
				FormattingLog.Error("Error formatting: " + @string, exception);
				return @string;
			}
		}
	}
}