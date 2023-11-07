using System;
using System.Collections.Generic;

namespace Albatross.Messaging.EventSource {
	public interface IEventReader {
		IEnumerable<LogEntry> ReadLast(TimeSpan span);
	}
}
