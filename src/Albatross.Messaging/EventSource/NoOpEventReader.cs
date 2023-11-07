using Albatross.Messaging.Messages;
using System;
using System.Collections.Generic;

namespace Albatross.Messaging.EventSource {
	public class NoOpEventReader : IEventReader {
		public IEnumerable<LogEntry> ReadLast(TimeSpan span) => Array.Empty<LogEntry>();
	}
}
