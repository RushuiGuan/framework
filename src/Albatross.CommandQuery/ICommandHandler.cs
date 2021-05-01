using System.Threading.Tasks;

namespace Albatross.CommandQuery {
	public interface ICommandHandler {
		Task Handle(Command command);
	}
	public interface ICommandHandler<T> : ICommandHandler{
		Task Handle(T command);
	}
}
