using NetMQ.Sockets;
using NetMQ;
using Microsoft.Extensions.Logging;
using Albatross.Messaging.Messages;
using System.ComponentModel;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.DataLogging;
using System;
using Albatross.Reflection;
using Albatross.Messaging.Services;
using Albatross.Messaging.ReqRep.Messages;

namespace Albatross.Messaging.ReqRep {
	public class RouterBrokerService : IBrokerService, IMessagingService, IDisposable {
		private readonly AtomicCounter<ulong> counter = new AtomicCounter<ulong>();
		private readonly BrokerConfiguration config;
		private readonly IMessageFactory messageFactory;
		private readonly ILogger<RouterBrokerService> logger;
		private readonly WorkerRegistry registry;
		private readonly IDataLogWriter persistence;
		private readonly TimeSpan heartbeatThreshold;
		private readonly RouterSocket socket;
		private readonly NetMQTimer timer;
		private readonly NetMQPoller poller;

		IDataLogWriter IMessagingService.DataLogger => persistence;

		bool disposed = false;
		public string Identity => config.Identity;


		public RouterBrokerService(BrokerConfiguration config, WorkerRegistry registry, IDataLogWriter persistence, IMessageFactory messageFactory, ILogger<RouterBrokerService> logger) {
			this.config = config;
			this.registry = registry;
			this.persistence = persistence;
			this.messageFactory = messageFactory;
			this.logger = logger;
			heartbeatThreshold = TimeSpan.FromMilliseconds(config.ActualHeartbeatThreshold);
			socket = new RouterSocket();
			socket.ReceiveReady += Socket_ReceiveReady;
			timer = new NetMQTimer(TimeSpan.FromMilliseconds(config.ActualHeartbeatInterval));
			timer.Elapsed += TimerElapsed;
			poller = new NetMQPoller { socket, timer };
		}


		/// <summary>
		/// TODO: when a worker is lost in the middle of a job.  how should the dealer behave?
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected void TimerElapsed(object? sender, NetMQTimerEventArgs args) {
			try {
				foreach (var worker in registry) {
					if (worker.IsActive) {
						var elapsed = DateTime.Now - worker.LastHeartbeat;
						if (elapsed > heartbeatThreshold) {
							worker.StateChange(WorkerState.Unavailable);
							logger.LogInformation("lost: {name}, {elapsed:#,#} > {threshold:#,#}", worker.Identity, elapsed.TotalMilliseconds, heartbeatThreshold.TotalMilliseconds);
						}
					}
				}
			} catch (Exception err) {
				logger.LogError(err, "error processing broker service timer event");
			}
		}
		private void Socket_ReceiveReady(object? sender, NetMQSocketEventArgs args) {
			try {
				var frames = args.Socket.ReceiveMultipartMessage();
				var msg = messageFactory.Create(true, frames);
				switch (msg) {
					case WorkerConnect connect:
						AcceptConnection(connect);
						break;
					case WorkerHeartbeat heartbeat:
						AcceptHeartbeat(heartbeat);
						break;
					case ClientRequest request:
						AcceptCommand(request);
						break;
					case WorkerResponse response:
						AcceptWorkerResponse(response);
						break;
					default:
						logger.LogInformation("unhandled router broker msg: {msg}", msg);
						break;
				}
			} catch (Exception err) {
				logger.LogError(err, "error processing broker message");
			}
		}

		private void AcceptWorkerResponse(WorkerResponse response) {
			if (registry.TryGetWorker(response.Route, out var worker)) {
				worker.LastHeartbeat = DateTime.Now;
				if (worker.IsActive) {
					// run the next job in queue
				} else {

				}
			}
		}

		private void AcceptCommand(ClientRequest request) {
			if (registry.TryFindNextAvailableWorker(request.Service, out var worker)) {
				if (config.AllowParallelExecution || worker.State == WorkerState.Ready) {
					this.Transmit(new BrokerRequest(worker.Identity, counter.NextId(), request.Route, request.Payload));
				} else {
					worker.Requests.Enqueue(request);
				}
			} else {
				this.Transmit(new NoAvailableWorker(request.Route, request.Id, request.Service));
			}
		}

		private void AcceptHeartbeat(WorkerHeartbeat heartbeat) {
			if (registry.TryGetWorker(heartbeat.Route, out var worker) && worker.IsActive) {
				worker.LastHeartbeat = DateTime.Now;
				this.Transmit(new ServerAck(heartbeat.Route, counter.NextId()));
			} else {
				this.Transmit(new Reconnect(heartbeat.Route, counter.NextId()));
			}
		}

		/// <summary>
		/// when a worker is reconnected, if there are any request in its queue, the broker will send the request instead of a ConnectOk message.
		/// </summary>
		/// <param name="msg"></param>
		private void AcceptConnection(WorkerConnect msg) {
			var worker = registry.Add(msg.Route, msg.Services);
			worker.LastHeartbeat = DateTime.Now;
			worker.StateChange(WorkerState.Connected);
			if (worker.Requests.Count > 0) {
				var request = worker.Requests.Dequeue();
				this.Transmit(new BrokerRequest(worker.Identity, counter.NextId(), request.Route, request.Payload));
			} else {
				this.Transmit(new BrokerConnectOk(msg.Route, counter.NextId(), config.ActualHeartbeatInterval, config.ActualHeartbeatThreshold));
			}
		}

		public void Start() {
			logger.LogInformation("starting broker and binding to router socket at {endpoint} as {identity}", config.EndPoint, Identity);
			socket.Bind(config.EndPoint);
			poller.RunAsync();
		}

		public void Dispose() {
			if (!disposed) {
				logger.LogInformation("closing and disposing broker socket");
				poller.Dispose();
				socket.Dispose();
				disposed = true;
			}
		}

		public void SubmitToQueue(object message) {
			throw new NotImplementedException();
		}
		public void Transmit(IMessage msg) {
			var frames = msg.Create();
			persistence.Outgoing(msg, frames);
			this.socket.SendMultipartMessage(frames);
		}
	}
}