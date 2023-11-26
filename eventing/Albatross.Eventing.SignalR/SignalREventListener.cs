using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Eventing.SignalR {
	public class SignalREventListener : IEventListener {
		public ValueTask DisposeAsync() {
			throw new NotImplementedException();
		}

		public Task Listen(string topic, Action<string, object> callback) {
			throw new NotImplementedException();
		}
	}
}
