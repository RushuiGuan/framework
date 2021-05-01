using System.Threading.Tasks;

namespace Albatross.CommandQuery {
	public interface ICommandHandler<T> {
		Task Handle(T command);
	}
}
