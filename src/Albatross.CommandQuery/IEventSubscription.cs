using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CommandQuery {
	public interface IEventSubscription<T> {
		string Subscriber { get; }
		Task Receive(T @event);
	}
}
