using System;
using System.Collections.Generic;

namespace Albatross.CommandQuery {
	public abstract class Command{
		public Guid Id { get; init; }
		public Command() {
			Id = Guid.NewGuid();
		}
	}
}
