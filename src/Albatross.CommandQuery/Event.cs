using System;

namespace Albatross.CommandQuery {
	public abstract class Event{
		public Guid Id { get; init; }
		public Event () {
			Id = Guid.NewGuid();
		}
	}
}
