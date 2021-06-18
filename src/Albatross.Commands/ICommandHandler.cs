using System;
using System.Threading.Tasks;

namespace Albatross.Commands {
	// command handlers should be registered as scoped
	public interface ICommandHandler {
		Task<object> Handle(Command command);
	}
	public interface ICommandHandler<T, K> : ICommandHandler where K:notnull where T:Command<K> {
		Task<K> Handle(T command);
	}
}
