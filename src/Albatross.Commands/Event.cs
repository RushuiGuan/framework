using System;

namespace Albatross.Commands {
	public abstract class Event{
		public Guid Id { get; init; }
		public Event () {
			Id = Guid.NewGuid();
		}
	}
}
