using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.CommandQuery {
	public interface ICommandQueue<T>: IDisposable where T:Command {
		string Name { get; }
		Task Submit(T command);
	}
	public class CommandQueue<T> : ICommandQueue<T> where T : Command {
		private object sync = new object();
		private readonly ILogger logger;
		private readonly Queue<T> queue = new Queue<T>();
		private readonly IServiceScopeFactory scopeFactory;
		private readonly Action<Queue<T>> collapseCommand;
		private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		private bool running = true;
		public string Name { get; init; }

		public CommandQueue(string name, 
			Action<Queue<T>> collapseCommand,
			IServiceScopeFactory scopeFactory, 
			ILogger<CommandQueue<T>> logger) {
			logger.LogInformation("creating command queue {name}", name);
			this.scopeFactory = scopeFactory;
			this.collapseCommand = collapseCommand;
			this.logger = logger;
			this.Name = name;
			Task.Run(Start);
		}
		
		public Task Submit(T command) {
			lock (sync) {
				logger.LogInformation("{queue} incoming:{@data}", this.Name, command);
				queue.Enqueue(command);
				this.collapseCommand?.Invoke(queue);
			}
			autoResetEvent.Set();
			return Task.CompletedTask;
		}

		async void Start() {
			logger.LogDebug("starting command queue {name}", this.Name);
			while (running) {
				autoResetEvent.WaitOne();
				if (running) {
					using var scope = scopeFactory.CreateScope();
					var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>();
					while (true) {
						T command;
						lock (sync) { 
							if (!queue.TryDequeue(out command)) {
								break;
							}
						}
						try {
							logger.LogInformation("{queue} processing: {id}",this.Name, command.Id);
							await commandHandler.Handle(command);
						} catch (Exception err) {
							logger.LogError(err, "{queue} failed to process: {id}", this.Name, command.Id);
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
