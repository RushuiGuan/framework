using System;
using System.Threading.Tasks;

namespace Albatross.Commands {
	public abstract class BaseCommandHandler<T, K> : ICommandHandler<T, K> where T : Command<K> where K :notnull {
		public abstract Task<K> Handle(T command);

		public async Task<object> Handle(Command command) {
			if(command is T) {
				K result = await this.Handle((T)command);
				return result;
			} else {
				throw new ArgumentException();
			}
		}
	}
}
