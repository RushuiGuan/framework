using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Messaging.Test {
	public class MockRouterServer : IMessagingService {
		public IEventWriter EventWriter => throw new NotImplementedException();

		public IAtomicCounter Counter => throw new NotImplementedException();

		public ClientState GetClientState(string identity) {
			throw new NotImplementedException();
		}

		public void SubmitToQueue(object message) {
			throw new NotImplementedException();
		}

		public void Transmit(IMessage msg) {
			throw new NotImplementedException();
		}
	}
}