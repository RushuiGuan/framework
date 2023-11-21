using System;

namespace Albatross.Messaging.Commands {
	public class CommandContext {
		public Guid ContextId { get; }
		public CommandContext() { 
			ContextId = Guid.NewGuid();
		}


		public ulong OriginalId { get; internal set; }
		public string OriginalRoute { get; internal set; } = string.Empty;
		public ulong Id { get;internal set; }
		public string Route { get; internal set; } = string.Empty;
		public string Queue { get; internal set; } = string.Empty;
		public bool IsInternal { get; internal set; }
	}
}
