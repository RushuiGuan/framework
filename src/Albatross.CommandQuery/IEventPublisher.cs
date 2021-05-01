using System.Threading.Tasks;

namespace Albatross.CommandQuery {
	public interface IEventPublisher<T> {
		Task Send(T @event);
	}
}
