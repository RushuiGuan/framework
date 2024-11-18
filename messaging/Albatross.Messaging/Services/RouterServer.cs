using Albatross.Collections;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Messaging.Services {
	public class RouterServer : IMessagingService, IDisposable {
		private readonly RouterServerConfiguration config;
		private readonly RouterSocket socket;
		private readonly NetMQPoller poller;
		private readonly NetMQQueue<object> queue;
		private readonly NetMQTimer timer;
		private readonly ILogger<RouterServer> logger;
		private readonly IMessageFactory messageFactory;
		private readonly DiskStorageEventWriter eventWriter;
		private readonly IEventReader eventReader;
		private bool running = false;
		private bool disposed = false;
		private IEnumerable<IRouterServerService> receiveServices;
		private IEnumerable<IRouterServerService> transmitServices;
		private IEnumerable<IRouterServerService> timerServices;
		private Dictionary<string, Client> clients = new Dictionary<string, Client>();
		private IAtomicCounter counter;
		private ulong timerCounter;

		public IEventWriter EventWriter => this.eventWriter;
		public IAtomicCounter Counter => this.counter;

		public RouterServer(RouterServerConfiguration config, IEnumerable<IRouterServerService> services, ILoggerFactory loggerFactory, IMessageFactory messageFactory) {
			this.logger = loggerFactory.CreateLogger<RouterServer>();
			logger.LogInformation("Creating {name} instance w. maintainConnection={maintain}, receiveHighWatermark={rchw}, sendHighWatermark={sendhw}, timerInterval={timerintv}",
				nameof(RouterServer), config.MaintainConnection, config.ReceiveHighWatermark, config.SendHighWatermark, config.ActualTimerInterval);
			this.config = config;
			this.receiveServices = services.Where(args => args.CanReceive).ToArray();
			this.transmitServices = services.Where(args => args.HasCustomTransmitObject).ToArray();
			this.timerServices = services.Where(args => args.NeedTimer).ToArray();
			this.messageFactory = messageFactory;
			this.counter = new DurableAtomicCounter(config.DiskStorage.WorkingDirectory);
			this.eventWriter = new DiskStorageEventWriter("router-server", config.DiskStorage, loggerFactory);
			this.eventReader = new DiskStorageEventReader(config.DiskStorage, messageFactory, loggerFactory.CreateLogger("router-server-log-reader"));
			this.socket = new RouterSocket();
			if (config.UseCurveEncryption) {
				if(string.IsNullOrEmpty(config.ServerPrivateKey)) {
					throw new ArgumentException("curve encryption is enabled but the server private key is missing");
				}
				socket.Options.CurveServer = true;
				socket.Options.CurveCertificate = NetMQCertificate.CreateFromSecretKey(config.ServerPrivateKey);
			}
			this.socket.ReceiveReady += Socket_ReceiveReady;
			this.socket.Options.ReceiveHighWatermark = config.ReceiveHighWatermark;
			this.socket.Options.SendHighWatermark = config.SendHighWatermark;
			this.queue = new NetMQQueue<object>();
			this.queue.ReceiveReady += Queue_ReceiveReady;
			this.timer = new NetMQTimer(config.ActualTimerInterval);
			this.timer.Elapsed += Timer_Elapsed;
			this.poller = new NetMQPoller { socket, queue, };
			if (timerServices.Any() || config.MaintainConnection) {
				this.poller.Add(timer);
			} else {
				timer.Enable = false;
			}
		}

		private void Timer_Elapsed(object? sender, NetMQTimerEventArgs e) {
			timerCounter++;
			if (config.MaintainConnection) {
				foreach (var client in this.clients.Values) {
					if (client.State != ClientState.Dead) {
						var elapsed = DateTime.UtcNow - client.LastHeartbeat;
						if (elapsed > config.HeartbeatThresholdTimeSpan) {
							client.Lost();
							logger.LogInformation("lost: {name}, {elapsed:#,#} > {threshold:#,#}", client.Identity, elapsed.TotalMilliseconds, config.HeartbeatThresholdTimeSpan.TotalMilliseconds);
						}
					}
				}
			}
			foreach (var service in this.timerServices) {
				try {
					service.ProcessTimerElapsed(this, timerCounter);
				} catch (Exception ex) {
					logger.LogError(ex, "error processing timer elapsed from {type}", service.GetType().FullName);
				}
			}
		}

		private void Queue_ReceiveReady(object? sender, NetMQQueueEventArgs<object> e) {
			try {
				if (running) {
					var item = e.Queue.Dequeue();
					switch (item) {
						case IMessage msg:
							this.Transmit(msg);
							break;
						default:
							foreach (var service in this.transmitServices) {
								if (service.ProcessQueue(this, item)) {
									return;
								}
							}
							if (!(item is ISystemMessage)) {
								logger.LogError("transmit queue item {@item} not processed", item);
							}
							break;
					}
				} else {
					logger.LogError("cannot process transmit queue item because the router server is being disposed");
				}
			} catch (Exception err) {
				logger.LogError(err, "error processing command reply");
			}
		}

		private void Socket_ReceiveReady(object? sender, NetMQSocketEventArgs e) {
			try {
				var frames = e.Socket.ReceiveMultipartMessage();
				var msg = this.messageFactory.Create(frames);
				if (msg is UnknownMsg) {
					logger.LogError("unknown message: {@msg}", msg);
					return;
				}
				this.eventWriter.WriteEvent(new EventEntry(EntryType.In, msg));
				if (msg is ClientAck) { return; }
				if (running) {
					if (msg is Connect connect) {
						AcceptConnection(connect);
					} else if (msg is Heartbeat heartbeat) {
						AcceptHeartbeat(heartbeat);
						return;
					}
					foreach (var service in receiveServices) {
						if (service.ProcessReceivedMsg(this, msg)) {
							return;
						}
					}
					if (!(msg is ISystemMessage)) {
						logger.LogInformation("unhandled router server msg: {msg}", msg);
					}
				} else {
					logger.LogError("incoming message {msg} cannot get processed because the router server is being disposed", msg);
				}
			} catch (Exception err) {
				logger.LogError(err, "error parsing router server message");
			}
		}

		private void AcceptConnection(Connect msg) {
			var client = clients.GetOrAdd(msg.Route, () => {
				logger.LogInformation("new client: {name}", msg.Route);
				return new Client(msg.Route);
			});
			this.Transmit(new ConnectOk(msg.Route, counter.NextId()));
			if (client.State == ClientState.Dead) {
				client.Connected();
				SendClientResumeMsgToService(client.Identity);
			}
		}

		private void SendClientResumeMsgToService(string client) {
			logger.LogInformation("client {name} came back from lost, sending resume signal", client);
			foreach (var service in receiveServices) {
				service.ProcessReceivedMsg(this, new Resume(client, counter.NextId()));
			}
		}

		private void AcceptHeartbeat(Heartbeat heartbeat) {
			if (clients.TryGetValue(heartbeat.Route, out var client)) {
				client.UpdateHeartbeat();
				Transmit(new HeartbeatAck(heartbeat.Route, counter.NextId()));
				if (client.State != ClientState.Alive) {
					SendClientResumeMsgToService(client.Identity);
				}
			} else {
				// receive a heartbeat from non existing client.  asking the client to reconnect
				this.Transmit(new Reconnect(heartbeat.Route, counter.NextId()));
			}
		}

		public void Start() {
			if (!running) {
				running = true;
				this.logger.LogInformation("starting router server at {endpoint}", config.EndPoint);
				this.socket.Bind(config.EndPoint);
				// wait a second here.  if we start transmitting messages right away, it will get lost
				Task.Delay(1000).Wait();

				logger.LogInformation("running log replay");
				int counter = 0;
				this.queue.Enqueue(new StartReplay());
				if (config.LogCatchUpPeriod > 0) {
					foreach (var eventEntry in this.eventReader.ReadLast(TimeSpan.FromMinutes(config.LogCatchUpPeriod))) {
						counter++;
						if (!(eventEntry.Message is ISystemMessage)) {
							this.queue.Enqueue(new Replay(eventEntry.Message, counter, eventEntry.EntryType));
						}
					}
				} else {
					logger.LogWarning("Log catch up period is configured to be 0.  Therefore log catch up functionality is disabled");
				}
				this.queue.Enqueue(new EndReplay());
				this.poller.RunAsync();
			}
		}

		public void SubmitToQueue(object result) => this.queue.Enqueue(result);

		public void Transmit(IMessage msg) {
			this.eventWriter.WriteEvent(new EventEntry(EntryType.Out, msg));
			var frames = msg.Create();
			this.socket.SendMultipartMessage(frames);
		}

		public ClientState GetClientState(string identity) {
			if (config.MaintainConnection) {
				if (clients.TryGetValue(identity, out var client)) {
					return client.State;
				} else {
					return ClientState.Dead;
				}
			} else {
				return ClientState.Alive;
			}
		}
		public void Dispose() {
			if (!disposed) {
				running = false;
				logger.LogInformation("closing and disposing router server");
				poller.Stop();
				poller.RemoveAndDispose(socket);
				poller.Dispose();
				queue.Dispose();
				eventWriter.Dispose();
				disposed = true;
				logger.LogInformation("router server disposed");
			}
		}
	}
}