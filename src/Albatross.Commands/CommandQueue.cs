using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Commands {
	/// <summary>
	/// command queue should be registered as transient.  Its instances are managed by the CommandBus
	/// </summary>
	public interface ICommandQueue: IDisposable {
		void Submit(Command command);
		void SetName(string name);
		Task Start();
	}
	public class CommandQueue : ICommandQueue {
		private readonly ILogger logger;
		private readonly IServiceScopeFactory scopeFactory;
		private object sync = new object();
		private readonly Queue<Command> queue = new Queue<Command>();
		private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		private bool running = false;

		public string Name { get; private set; } = string.Empty;
		

		/// <summary>
		/// this is not a good thing to do, but we don't have a better alternative.
		/// </summary>
		/// <param name="name"></param>
		public void SetName(string name) {
			if (string.IsNullOrEmpty(this.Name)) {
				this.Name = name;
				logger.LogInformation("creating command queue {name}", Name);
			} else {
				throw new InvalidOperationException($"The name of command queue {this.Name} has already been set");
			}
		}

		public CommandQueue(IServiceScopeFactory scopeFactory, ILogger<CommandQueue> logger) {
			this.scopeFactory = scopeFactory;
			this.logger = logger;
		}

		
		public virtual void Submit(Command command) {
			lock (sync) {
				logger.LogInformation("submitted {name}: {@data}", Name, command);
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