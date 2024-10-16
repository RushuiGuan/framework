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
		public int Invoke(InvocationContext context) => throw new NotSupportedException();
		public virtual Task<int> InvokeAsync(InvocationContext context) {
			logger.LogInformation("Running Command {name}", context.ParsedCommandName());
			return Task.FromResult(0);
		}
	}
}
