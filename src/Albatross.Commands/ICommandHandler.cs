using System.Threading.Tasks;

namespace Albatross.Commands {
	// command handlers should be registered as scoped
	public interface ICommandHandler {
		Task Handle(Command command);
	}
	public interface ICommandHandler<T> : ICommandHandler where T:Command {
		Task Handle(T command);
	}
}
