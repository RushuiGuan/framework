using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Commands {
	// a couple problems with this implementation
	// when disposing, the remaining commands in the queue will be disgarded
	// should create an explicit close command.  The close command will block until the queue is drained
	public class CommandQueue : ICommandQueue {
		private readonly ILogger logger;
		private readonly IServiceScopeFactory scopeFactory;
		private bool running = false;

		protected object sync = new object();
		protected readonly Queue<Command> queue = new Queue<Command>();
		protected readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);

		public string Name { get; init; }
		public Command? Last { get; private set; }

		public int Count {
			get {
				lock (sync) {
					return queue.Count;
				}
			}
		}

		public CommandQueue(string name, IServiceScopeFactory scopeFactory, ILoggerFactory loggerFactory) {
			this.Name = name;
			this.scopeFactory = scopeFactory;
			this.logger = loggerFactory.CreateLogger($"CommandQueue:{name}");
			logger.LogInformation("creating command queue {name}", Name);
		}


		public virtual void Submit(Command command) {
			lock (sync) {
				logger.LogInformation("submitted {name}: {id}", Name, command.Id);
				queue.Enqueue(command);
				command.MarkSubmitted();
			}
			autoResetEvent.Set();
		}

		public async Task Start() {
			try {
				lock (sync) {
					if (running) {
						logger.LogWarning("command queue {name} is already running!", Name);
						return;
					} else {
						running = true;
					}
				}
				logger.LogDebug("starting {name}", Name);
				while (running) {
					autoResetEvent.WaitOne();
					if (running) {
						while (true) {
							Command? command;
							lock (sync) {
								if (!queue.TryDequeue(out command)) {
									break;
								} else {
									Last = command;
								}
							}
							try {
								using (var scope = scopeFactory.CreateScope()) {
									var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), command.ReturnType);
									var commandHandler = (ICommandHandler)scope.ServiceProvider.GetRequiredService(handlerType);
									logger.LogInformation("processing {name}: {commandId}", Name, command.Id);
									command.MarkStart();
									var result = await commandHandler.Handle(command).ConfigureAwait(false);
									command.SetResult(result);
									logger.LogInformation("processed {name}: {commandId}", Name, command.Id);
								}
							} catch (Exception err) {
								logger.LogError(err, "failed to process {name}: {commandId}", Name, command.Id);
								command.SetException(err);
							}
						}
					}
				}
				logger.LogInformation("{name} terminated", Name);
			} catch (Exception err) {
				logger.LogCritical($"Command Queue {Name} crashed", err);
			}
		}

		public void Dispose() {
			logger.LogInformation("{name} shutting down", Name);
			lock (sync) {
				running = false;
			}
			autoResetEvent.Set();
			autoResetEvent.Dispose();
		}

		public void Signal() => this.autoResetEvent.Set();

		public CommandQueueDto CreateDto() {
			return new CommandQueueDto(this.Name, this.running, this.queue.Select(args => args.CreateDto()).ToArray(), this.Last?.CreateDto());
		}
	}
}