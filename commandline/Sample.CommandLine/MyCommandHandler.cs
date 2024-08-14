using Microsoft.Extensions.Logging;
using System.CommandLine.Invocation;

namespace Sample.CommandLine {
	public class MyCommandHandler : ICommandHandler {
		private readonly ILogger<MyCommandHandler> logger;

		public MyCommandHandler(ILogger<MyCommandHandler> logger) {
			this.logger = logger;
		}

		public int Invoke(InvocationContext context) {
			throw new NotSupportedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			logger.LogInformation("i am here");
			return Task.FromResult(0);
		}
	}
}
