using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Commands {
	public class CommandQueue : ICommandQueue {
		private readonly ILogger logger;
		private readonly IServiceScopeFactory scopeFactory;
		private bool running = false;

		protected object sync = new object();
		protected readonly Queue<Command> queue = new Queue<Command>();
		protected readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		
		public string Name { get; init; }
		
		public CommandQueue(string name, IServiceScopeFactory scopeFactory, ILogger<CommandQueue> logger) {
			this.Name = name;
			this.scopeFactory = scopeFactory;
			this.logger = logger;
			logger.LogInformation("creating command queue {name}", Name);
		}

		public virtual void Submit(Command command) {
			lock (sync) {
				logger.LogInformation("submitted {name}: {id}", Name, command.Id);
				queue.Enqueue(command);
			}
			autoResetEvent.Set();
			command.Wait();
		}

		public async Task  Start() {
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
							}
						}
						try {
							using (var scope = scopeFactory.CreateScope()) {
								var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
								var commandHandler = (ICommandHandler)scope.ServiceProvider.GetRequiredService(handlerType);
								logger.LogInformation("processing {name}: {commandId}", Name, command.Id);
								await commandHandler.Handle(command).ConfigureAwait(false);
								logger.LogInformation("processed {name}: {commandId}", Name, command.Id);
							}
						} catch (Exception err) {
							logger.LogError(err, "failed to process {name}: {commandId}", Name, command.Id);
							command.Error(err);
						} finally {
							command.Complete();
						}
					}
				}
			}
			logger.LogInformation("{name} terminated", Name);
		}

		public void Dispose() {
			logger.LogInformation("{name} shutting down", Name);
			lock (sync) {
				running = false;
			}
			autoResetEvent.Set();
			autoResetEvent.Dispose();
		}
	}
}