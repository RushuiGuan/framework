using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Eventing {
	public interface IEventSource {
		Task Send<T>(ClientType clientType, string client, string topic, object[] data, CancellationToken cancellationToken);
	}
}