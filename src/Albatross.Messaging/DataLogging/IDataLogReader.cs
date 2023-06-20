using Albatross.Messaging.Messages;
using System;
using System.Collections.Generic;

namespace Albatross.Messaging.DataLogging {
	public interface IDataLogReader {
		IEnumerable<DataLog> ReadLast(TimeSpan span);
	}
}
