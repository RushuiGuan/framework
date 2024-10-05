using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("test", typeof(Test))]
	public class TestOptions { }

	public class Test :ICommandHandler {
		private readonly ILogger<Test> logger;

		public Test(ILogger<Test> logger) {
			this.logger = logger;
		}
		public int Invoke(InvocationContext context) { throw new System.NotImplementedException(); }

		public Task<int> InvokeAsync(InvocationContext context) {
			logger.LogInformation("An info msg");
			logger.LogInformation("A warning msg");
			logger.LogInformation("An err msg");
			return Task.FromResult(0);
		}
	}
}
