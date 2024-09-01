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

		public Task<int> InvokeAsync(InvocationContext context) {
			var provider = context.GetHost().Services;
			var logger = provider.GetRequiredService<ILogger<CommandHandlerFactory>>();
			var handler = provider.GetKeyedService<ICommandHandler>(command.Name);
			var globalOptions = provider.GetRequiredService<IOptions<GlobalOptions>>().Value;
			if (handler == null) {
				logger.LogError("No CommandHandler is not registered for Command {command}", command.Name);
				return Task.FromResult(1);
			} else {
				Stopwatch? stopwatch;
				if (globalOptions.Benchmark) {
					stopwatch = Stopwatch.StartNew();
				} else {
					stopwatch = null;
				}
				try {
					return handler.InvokeAsync(context);
				} catch (Exception err) {
					if (globalOptions.ShowStack) {
						logger.LogError(err, "Error invoking Command {command}", command.Name);
					} else {
						logger.LogError("Error invoking Command {command}: {message}", command.Name, err.Message);
					}
					return Task.FromResult(1);
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
