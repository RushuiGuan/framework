using Albatross.Messaging.Messages;
using System;
using System.Collections.Generic;

namespace Albatross.Messaging.DataLogging {
	public interface IDataLogReader {
		IEnumerable<IMessage> ReadLast(TimeSpan span);
	}
}
