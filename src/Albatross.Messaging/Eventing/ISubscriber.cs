using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing {
	public interface ISubscriber {
		Task DataReceived(string topic, byte[] data);
	}
}
