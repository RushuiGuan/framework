using System.Threading.Tasks;

namespace Albatross.Commands {
	public interface IEventPublisher<T> {
		Task Send(T @event);
	}
}
