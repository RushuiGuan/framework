using System.Threading.Tasks;

namespace Albatross.Commands {
	public interface ICommandHandler {
		Task Handle(Command command);
	}
	public interface ICommandHandler<T> : ICommandHandler{
		Task Handle(T command);
	}
}
