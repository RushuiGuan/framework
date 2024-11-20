using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Utility {
	public class EventEntry<T> where T : IMessage {
		public EventEntry(EventEntry eventEntry) {
			Entry = eventEntry;
		}
		public EventEntry Entry { get; }
		public T Message => (T)Entry.Message;
	}
}