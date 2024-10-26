namespace Albatross.Messaging.Messages {
	public interface ISystemMessage { }

	public class StartReplay : ISystemMessage { }
	public class Replay : ISystemMessage {
		public int Index { get; }
		public EntryType EntryType { get; }
		public Replay(IMessage message, int order, EntryType entryType) {
			Message = message;
			Index = order;
			EntryType = entryType;
		}
		public IMessage Message { get; }
	}
	public class EndReplay : ISystemMessage { }
}