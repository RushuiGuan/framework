using System.Collections.Generic;

namespace Albatross.Commands {
	public class CommandQueueDto {
		public string Name { get; init; }
		public bool IsRunning { get; init; }
		public IEnumerable<string> Items { get; init;  }

		public CommandQueueDto(string name, bool running, IEnumerable<string> items) {
			this.Name = name;
			this.IsRunning = running;
			this.Items = items;
		}

	}
}
