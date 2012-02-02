namespace QuickFix.Log4Net
{
	public class Log4NetLogFactory : LogFactory
	{
		public bool RichMessageLogging { get; set; }

		public Log4NetLogFactory()
		{
			RichMessageLogging = true;
		}

		public virtual Log Create(SessionID sessionId)
		{
			var log = new Log4NetLog(sessionId.ToString());
			log.RichMessageLogging = RichMessageLogging;
			return log;
		}
	}
}