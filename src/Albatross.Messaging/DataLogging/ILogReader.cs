using System;
using System.Collections.Generic;

namespace Albatross.Messaging.DataLogging {
	public interface ILogReader {
		IEnumerable<LogEntry> ReadLast(TimeSpan span);
	}
}
