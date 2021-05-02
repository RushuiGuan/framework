using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Commands {
	public abstract class BaseCommandHandler<T> : ICommandHandler<T> where T : Command {
		public abstract Task Handle(T command);

		Task ICommandHandler.Handle(Command command) {
			if(command is T) {
				return this.Handle((T)command);
			} else {
				throw new ArgumentException();
			}
		}
	}
}
