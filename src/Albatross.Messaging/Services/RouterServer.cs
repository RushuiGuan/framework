﻿using Albatross.Collections;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.DataLogging;
using Microsoft.Extensions.Logging;
using NetMQ.Sockets;
using NetMQ;
using System.Collections.Generic;
using System;
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
		private readonly ILogWriter logWriter;
		private readonly ILogReader logReader;
		private bool running = false;
		private bool disposed = false;
		private IEnumerable<IRouterServerService> receiveServices;
		private IEnumerable<IRouterServerService> transmitServices;
		private IEnumerable<IRouterServerService> timerServices;
		private Dictionary<string, Client> clients  = new Dictionary<string, Client>();
		private AtomicCounter<ulong> counter = new AtomicCounter<ulong>();


		public ILogWriter DataLogger => this.logWriter;
		public AtomicCounter<ulong> Counter => this.counter;

		public RouterServer(RouterServerConfiguration config, IEnumerable<IRouterServerService> services, ILogger<RouterServer> logger, IMessageFactory messageFactory, RouterServerLogWriter logWriter, RouterServerLogReader logReader) {
			logger.LogInformation($"Creating {nameof(RouterServer)} instance");
			this.config = config;
			this.receiveServices = services.Where(args => args.CanReceive).ToArray();
			this.transmitServices = services.Where(args=>args.HasCustomTransmitObject).ToArray();
			this.timerServices = services.Where(args=>args.NeedTimer).ToArray();
			this.messageFactory = messageFactory;
			this.logWriter = logWriter;
			this.logReader = logReader;
			this.logger = logger;
			this.socket = new RouterSocket();
			this.socket.ReceiveReady += Socket_ReceiveReady;
			this.socket.Options.ReceiveHighWatermark = config.ReceiveHighWatermark;
			this.socket.Options.SendHighWatermark = config.SendHighWatermark;
			this.queue = new NetMQQueue<object>();
			this.queue.ReceiveReady += Queue_ReceiveReady;
			this.timer = new NetMQTimer(config.ActualTimerInterval);
			this.timer.Elapsed += Timer_Elapsed;
			this.poller = new NetMQPoller { socket, queue, };
			if(timerServices.Any() || config.MaintainConnection) {
				this.poller.Add(timer);
			} else {
				timer.Enable = false;
			}
		}

		private void Timer_Elapsed(object? sender, NetMQTimerEventArgs e) {
			if (config.MaintainConnection) {
				foreach(var client in this.clients.Values) {
					if (client.State != ClientState.Dead) {
						var elapsed = DateTime.UtcNow - client.LastHeartbeat;
						if (elapsed > config.HeartbeatThresholdTimeSpan) {
							client.Lost();
							logger.LogInformation("lost: {name}, {elapsed:#,#} > {threshold:#,#}", client.Identity, elapsed.TotalMilliseconds, config.HeartbeatThresholdTimeSpan.TotalMilliseconds);
						}
					}
				}
			}
			foreach(var service in this.timerServices) {
				try {
					service.ProcessTimerElapsed(this);
				}catch(Exception ex) {
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
								if (service.ProcessTransmitQueue(this, item)) {
									return;
								}
							}
							if (!(item is ISystemMessage)) {
								logger.LogError("transmit queue item @{item} not processed", item);
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
				this.logWriter.WriteLogEntry(new LogEntry(EntryType.In, msg));
				if(msg is ClientAck) { return; }
				if (running) {
					if(msg is Connect connect) {
						AcceptConnection(connect);
					}else if(msg is Heartbeat heartbeat) {
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
			client.Connected();
			this.Transmit(new ConnectOk(msg.Route, counter.NextId()));
		}

		private void AcceptHeartbeat(Heartbeat heartbeat) {
			if(clients.TryGetValue(heartbeat.Route, out var client)) {
				if (client.State == ClientState.Alive) {
					client.UpdateHeartbeat();
					Transmit(new HeartbeatAck(heartbeat.Route, counter.NextId()));
				} else {
					//TODO:
					// receive a heartbeat from a dead client
					// send out a resume signal to services
				}
			} else {
				//TODO:
				// receive a heartbeat from non existing client.  asking the client to reconnect
				this.Transmit(new Reconnect(heartbeat.Route, counter.NextId()));
			}
		}

		public async Task Start() {
			this.logger.LogInformation("starting router server at {endpoint}", config.EndPoint);
			this.socket.Bind(config.EndPoint);
			// wait a second here.  if we start transmitting messages right away, it will get lost
			await Task.Delay(1000);
			
			logger.LogInformation("running log replay");
			int counter = 0;
			this.queue.Enqueue(new StartReplay());
			foreach (var dataLog in this.logReader.ReadLast(TimeSpan.FromMinutes(config.LogCatchUpPeriod))) {
				counter++;
				if (!(dataLog.Message is ISystemMessage)) {
					this.queue.Enqueue(new Replay(dataLog.Message, counter, dataLog.EntryType));
				}
			}
			this.queue.Enqueue(new EndReplay());
			if (!running) {
				running = true;
				this.poller.RunAsync();
			}
		}

		public void SubmitToQueue(object result) => this.queue.Enqueue(result);

		public void Transmit(IMessage msg) {
			this.logWriter.WriteLogEntry(new LogEntry(EntryType.Out, msg));
			var frames = msg.Create();
			this.socket.SendMultipartMessage(frames);
		}

		public void Dispose() {
			if (!disposed) {
				running = false;
				logger.LogInformation("closing and disposing router server");
				poller.Stop();
				poller.RemoveAndDispose(socket);
				poller.Dispose();
				queue.Dispose();
				disposed = true;
				logger.LogInformation("router server disposed");
			}
		}

		public ClientState GetClientState(string identity) {
			if (config.MaintainConnection) {
				if(clients.TryGetValue(identity, out var client)) {
					return client.State;
				} else {
					return ClientState.Dead;
				}
			} else {
				return ClientState.Alive;
			}
		}
	}
}