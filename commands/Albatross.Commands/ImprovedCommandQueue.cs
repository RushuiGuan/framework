using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Commands {
	public class ImprovedCommandQueue : ICommandQueue {
		private readonly ILogger logger;
		private readonly IServiceScopeFactory scopeFactory;
		private bool running = false;

		protected object sync = new object();
		protected readonly Queue<Command> queue = new Queue<Command>();

		public string Name { get; init; }
		public Command? Last { get; private set; }

		public int Count {
			get {
				lock (sync) {
					return queue.Count;
				}
			}
		}

		public ImprovedCommandQueue(string name, IServiceScopeFactory scopeFactory, ILoggerFactory loggerFactory) {
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
				if (!running) {
					running = true;
					_ = Start();
				}
			}
		}

		public async Task Start() {
			try {
				while (true) {
					Command? command;
					lock (sync) {
						if (!queue.TryDequeue(out command)) {
							logger.LogInformation("Command Queue {name} is finished", Name);
							running = false;
							return;
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
			} catch (Exception err) {
				logger.LogCritical($"Command Queue {Name} crashed", err);
			} finally {
				lock (sync) {
					running = false;
				}
			}
		}

		public CommandQueueDto CreateDto() {
			return new CommandQueueDto(this.Name, this.running, this.queue.Select(args => args.CreateDto()).ToArray(), this.Last?.CreateDto());
		}

		public void Signal() { _ = Start(); }
		
		public void Dispose() { }

	}
}