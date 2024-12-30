using System;

namespace Albatross.Commands {
	public abstract class Event{
		public string Id { get; init; }
		public Event (string? id) {
			if (id == null || id == string.Empty) {
				Id = Guid.NewGuid().ToString();
			} else {
				Id = id;
			}
		}
	}
}
