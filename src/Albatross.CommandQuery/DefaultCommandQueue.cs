using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.CommandQuery {
	public interface ICommandQueue: IDisposable {
		void Submit(Command command);
		Task Start();
		string Name { get; }
	}
	public class DefaultCommandQueue : ICommandQueue {
		private readonly ILogger logger;
		private readonly IServiceScopeFactory scopeFactory;
		private object sync = new object();
		private readonly Queue<Command> queue = new Queue<Command>();
		private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		private bool running = true;
		public string Name => "Default";

		public DefaultCommandQueue(IServiceScopeFactory scopeFactory, ILogger<DefaultCommandQueue> logger) {
			logger.LogInformation("creating command queue {name}", Name);
			this.scopeFactory = scopeFactory;
			this.logger = logger;
		}
		
		public void Submit(Command command) {
			lock (sync) {
				logger.LogInformation("{queue} incoming:{@data}", this.Name, command);
				queue.Enqueue(command);
			}
			autoResetEvent.Set();
			command.Wait();
		}

		public async Task  Start() {
			logger.LogDebug("starting command queue {name}", this.Name);
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
							using var scope = scopeFactory.CreateScope();
							var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
							var commandHandler = (ICommandHandler)scope.ServiceProvider.GetRequiredService(handlerType);
							logger.LogInformation("{queue} processing: {id}",this.Name, command.Id);
							await commandHandler.Handle(command).ConfigureAwait(false);
						} catch (Exception err) {
							logger.LogError(err, "{queue} failed to process: {id}", this.Name, command.Id);
							command.Fail(err);
						} finally {
							command.Complete();
						}
					}
				}
			}
			logger.LogInformation("Command queue terminated {name}", this.Name);
		}

		public void Dispose() {
			logger.LogInformation("shutting down command queue {name}", this.Name);
			running = false;
			autoResetEvent.Set();
			autoResetEvent.Dispose();
		}
	}
}
