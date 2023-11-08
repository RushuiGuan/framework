using System;

namespace Albatross.Messaging.Commands {
	public class CommandContext {
		public Guid Id { get; }
		public CommandContext() { 
			Id = Guid.NewGuid();
		}
		public string Queue { get; internal set; } = string.Empty;
		public ulong OriginalId { get; internal set; }
		public string OriginalRoute { get; internal set; } = string.Empty;
		public bool IsInternal { get; internal set; }
	}
}
