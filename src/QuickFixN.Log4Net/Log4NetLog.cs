namespace QuickFix.Log4Net
{
	public class Log4NetLog : Log4NetLogBase, Log
	{
		public Log4NetLog()
		{
		}

		public Log4NetLog(string sessionId)
			: base(sessionId)
		{
		}


		public virtual void Clear()
		{
		}

		public virtual void Backup()
		{
		}

		public virtual void OnIncoming(string @string)
		{
			Log.Info("Incoming: " + GetMessageParsed(@string));
		}

		public virtual void OnOutgoing(string @string)
		{
			Log.Info("Outgoing: " + GetMessageParsed(@string));
		}

		public virtual void OnEvent(string @string)
		{
			Log.Info("Event: " + @string);
		}
	}
}