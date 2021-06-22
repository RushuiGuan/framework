using System;
using System.Collections.Generic;

namespace Albatross.Commands {
	public class CommandDto {
		public CommandDto(string id, Type returnType) {
			this.Id = id;
			this.ReturnType = returnType?.FullName ?? throw new ArgumentException();
		}
		public string Id { get; init; }
		public string ReturnType { get; init; }

		public DateTime? StartDateTimeUtc { get; set; }
		public DateTime? SubmitedDateTimeUtc { get; set; }
		public double? WaitTime { get; set; }
		public double? RunTime { get; set; }
		public bool Success { get; set; }
	}
	public class CommandQueueDto {
		public string Name { get; init; }
		public bool IsRunning { get; init; }
		public IEnumerable<CommandDto> Items { get; init;  }
		public CommandDto? Last { get; init; }

		public CommandQueueDto(string name, bool running, IEnumerable<CommandDto> items, CommandDto? last) {
			this.Name = name;
			this.IsRunning = running;
			this.Items = items;
			this.Last = last;
		}
	}
}
