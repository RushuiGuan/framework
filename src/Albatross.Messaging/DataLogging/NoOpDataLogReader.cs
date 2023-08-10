using Albatross.Messaging.Messages;
using System;
using System.Collections.Generic;

namespace Albatross.Messaging.DataLogging {
	public class NoOpDataLogReader : ILogReader {
		public IEnumerable<LogEntry> ReadLast(TimeSpan span) => Array.Empty<LogEntry>();
	}
}
