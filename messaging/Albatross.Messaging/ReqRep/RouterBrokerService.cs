using Albatross.Messaging.Messages;
using Albatross.Messaging.ReqRep.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Messaging.ReqRep {
	public class RouterBrokerService : IRouterServerService {
		private readonly ILogger<RouterBrokerService> logger;
		private readonly WorkerRegistry registry;

		public bool CanReceive => true;
		public bool HasCustomTransmitObject => throw new NotImplementedException();
		public bool NeedTimer => throw new NotImplementedException();

		public RouterBrokerService(WorkerRegistry registry, ILogger<RouterBrokerService> logger) {
			this.registry = registry;
			this.logger = logger;
		}

		private void AcceptConnection(Connect msg, IMessagingService routerServer) {
			//var worker = registry.Add(msg.Route, msg.Services);
			//worker.LastHeartbeat = DateTime.UtcNow;
			//worker.State = WorkerState.Connected;
			//routerServer.Transmit(new ConnectOk(msg.Route, msg.Id));
		}

		private void AcceptRequest(ClientRequest request, IMessagingService routerServer) {
			if (registry.TryFindNextAvailableWorker(request.Service, out var worker)) {
				var brokerRequest = new BrokerRequest(worker.Identity, routerServer.Counter.NextId(), request.Route, request.Id, request.Payload);
				routerServer.Transmit(brokerRequest);
				worker.Requests.Add(request);
			} else {
				routerServer.Transmit(new NoAvailableWorker(request.Route, request.Id, request.Service));
			}
		}

		private void AcceptWorkerResponse(WorkerResponse response, IMessagingService routerServer) {
			// forward it back to client
			routerServer.Transmit(new BrokerResponse(response.ClientId, response.RequestId, response.Payload));
			if (registry.TryGetWorker(response.Route, out var worker)) {
				worker.LastHeartbeat = DateTime.UtcNow;
			} else {
				// if the worker is not found. send a reconnect command
				routerServer.Transmit(new Reconnect(response.Route, routerServer.Counter.NextId()));
			}
		}

		public bool ProcessReceivedMsg(IMessagingService routerServer, IMessage msg) {
			switch (msg) {
				case Connect connect:
					AcceptConnection(connect, routerServer);
					return true;
				case ClientRequest request:
					AcceptRequest(request, routerServer);
					return true;
				case WorkerResponse response:
					AcceptWorkerResponse(response, routerServer);
					return true;
				default:
					return false;
			}
		}

		public bool ProcessQueue(IMessagingService routerServer, object msg) {
			return false;
		}
		public void ProcessTimerElapsed(IMessagingService routerServer, ulong count) {
			// check workers and see if any one of them is dead
		}
	}
}