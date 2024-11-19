using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.CommandLine {
	public class BaseHandler<T> : ICommandHandler where T : class {
		protected readonly T options;
		protected readonly ILogger logger;
		protected virtual TextWriter writer => Console.Out;

		public BaseHandler(IOptions<T> options, ILogger logger) {
			this.options = options.Value;
			this.logger = logger;
		}
		public virtual int Invoke(InvocationContext context) {
			logger.LogInformation("Running Command {name} of type {type} with options {@param}",
				context.ParsedCommandName(),
				context.ParseResult.CommandResult.Command.GetType().Name,
				this.options);
			return 0;
		}
		public virtual Task<int> InvokeAsync(InvocationContext context) {
			return Task.FromResult(Invoke(context));
		}
	}
}