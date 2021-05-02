using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CommandQuery {
	public abstract class BaseCommandHandler<T> : ICommandHandler<T> where T : Command {
		public abstract Task Handle(T command);
		public Task Handle(Command command) {
			if(command is T) {
				return this.Handle((T)command);
			} else {
				throw new ArgumentException($"Invalid command type, expected: {typeof(T).Name}, received: {command.GetType().Name}");
			}
		}
	}
}
