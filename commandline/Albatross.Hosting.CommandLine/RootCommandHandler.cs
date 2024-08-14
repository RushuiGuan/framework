using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Albatross.Hosting.CommandLine {
	public class RootCommandHandler : ICommandHandler {
		public int Invoke(InvocationContext context) {
			System.Console.WriteLine("root command invoked");
			return 0;
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			return Task.FromResult(Invoke(context));
		}
	}
}
