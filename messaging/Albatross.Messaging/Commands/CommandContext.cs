using Albatross.Messaging.Commands.Messages;
using System;
using System.Collections.Generic;

namespace Albatross.Messaging.Commands {
	public record class CommandContext {
		public Guid ContextId { get; }
		public CommandContext() {
			ContextId = Guid.NewGuid();
		}

		public ulong Id { get; internal set; }
		public string Route { get; internal set; } = string.Empty;
		public string Queue { get; internal set; } = string.Empty;
		public CommandMode Mode { get; set; }

		public List<ulong> InternalCommands { get; } = new List<ulong>();
	}
}