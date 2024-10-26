using System;
using System.Threading.Tasks;

namespace Albatross.Eventing {
	public interface IEventListener : IAsyncDisposable {
		Task Listen(string topic, Action<string, object> callback);
	}
}