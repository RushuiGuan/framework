using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing.Sub {
	public interface ISubscriber : IEquatable<ISubscriber>{
		Task DataReceived(string topic, byte[] data);
	}
}
