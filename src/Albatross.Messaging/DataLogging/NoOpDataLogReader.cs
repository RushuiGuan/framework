using Albatross.Messaging.Messages;
using System;
using System.Collections.Generic;

namespace Albatross.Messaging.DataLogging {
	public class NoOpDataLogReader : IDataLogReader {
		public IEnumerable<IMessage> ReadLast(TimeSpan span) => Array.Empty<IMessage>();
	}
}
