﻿namespace Albatross.Messaging.Messages {
	public interface ISystemMessage { }

	public class StartReplay : ISystemMessage { }
	public class Replay : ISystemMessage {
		public int Index { get; }
		public MessageDirection Direction { get; }
		public Replay(IMessage message, int order, MessageDirection direction) {
			Message = message;
			Index = order;
			Direction = direction;
		}
		public IMessage Message { get; }
	}
	public class EndReplay : ISystemMessage { }
}
