namespace Albatross.Messaging.Messages {
	public interface ISystemMessage { }

	public class StartReplay : ISystemMessage { }
	public class Replay : ISystemMessage {
		public int Index { get; }
		public Replay(IMessage message, int order) {
			Message = message;
			Index = order;
		}
		public IMessage Message { get; }
	}
	public class EndReplay : ISystemMessage { }
}
