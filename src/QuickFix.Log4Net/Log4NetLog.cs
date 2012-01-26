namespace QuickFix.Log4Net
{
	using System;
	using log4net;

	public class Log4NetLog : Log
	{
		public readonly ILog Log;

		public static string LoggerName = "QuickFix";

		public Log4NetLog()
		{
			Log = LogManager.GetLogger(LoggerName);
		}

		public Log4NetLog(SessionID sessionId)
		{
			// todo some other way to append session id information?
			Log = LogManager.GetLogger(LoggerName + "-" + sessionId);
		}

		public bool RichMessageLogging { get; set; }

		public virtual void clear()
		{
		}

		public virtual void backup()
		{
		}

		public virtual void onIncoming(string @string)
		{
			Log.Info("Incoming: " + GetMessageParsed(@string));
		}

		protected virtual string GetMessageParsed(string @string)
		{
			if (!RichMessageLogging)
			{
				return @string;
			}
			try
			{
				return new RichMessageFormatter().Format(@string);
			}
			catch (Exception)
			{
				return @string;
			}
		}

		public virtual void onOutgoing(string @string)
		{
			Log.Info("Outgoing: " + GetMessageParsed(@string));
		}

		public virtual void onEvent(string @string)
		{
			Log.Info("Event: " + @string);
		}
	}
}