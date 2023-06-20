using Albatross.Messaging.Messages;
using System;
using System.Collections.Generic;

namespace Albatross.Messaging.DataLogging {
	public class NoOpDataLogReader : IDataLogReader {
		public IEnumerable<DataLog> ReadLast(TimeSpan span) => Array.Empty<DataLog>();
	}
}
