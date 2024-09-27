using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Albatross.CommandLine {
	public class CommandHandlerFactory : ICommandHandler {
		private readonly Command command;

		public CommandHandlerFactory(Command command) {
			this.command = command;
		}

		public int Invoke(InvocationContext context) => throw new NotSupportedException();

		public async Task<int> InvokeAsync(InvocationContext context) {
			var provider = context.GetHost().Services;
			var globalOptions = provider.GetRequiredService<IOptions<GlobalOptions>>().Value;
			var logger = provider.GetRequiredService<ILogger<CommandHandlerFactory>>();
			ICommandHandler? handler = null;
			try {
				handler = provider.GetKeyedService<ICommandHandler>(command.Name);
			} catch (Exception err) {
				if (globalOptions.ShowStack) {
					logger.LogError(err, "Error creating CommandHandler for Command {command}", command.Name);
				} else {
					logger.LogError("Error creating CommandHandler for Command {command}: {msg}", command.Name, err.Message);
				}
				return 2;
			}
			if (handler == null) {
				logger.LogError("No CommandHandler is not registered for Command {command}", command.Name);
				return 1;
			} else {
				Stopwatch? stopwatch;
				if (globalOptions.Benchmark) {
					stopwatch = Stopwatch.StartNew();
				} else {
					stopwatch = null;
				}
				try {
					return await handler.InvokeAsync(context);
				} catch (Exception err) {
					if (globalOptions.ShowStack) {
						logger.LogError(err, "Error invoking Command {command}", command.Name);
					} else {
						logger.LogError("Error invoking Command {command}: {message}", command.Name, err.Message);
					}
					return 1;
				} finally {
					if (stopwatch != null) {
						stopwatch.Stop();
						logger.LogInformation("Command {command} took {time:#,#0} ms", command.Name, stopwatch.ElapsedMilliseconds);
					}
				}
			}
		}
	}
}
