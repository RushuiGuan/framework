using System;
using System.Collections.Generic;

namespace Albatross.Messaging.EventSource {
	public interface IEventReader {
		IEnumerable<EventEntry> ReadLast(TimeSpan span);
	}
}