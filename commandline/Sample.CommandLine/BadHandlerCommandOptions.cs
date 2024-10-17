using Albatross.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("invalid-constructor", typeof(BadHandlerCommandHandler),
		Description = "This handler cannot be constructed by dependency injection because of its invalid constructor")]
	public class BadHandlerCommandOptions { }

	public class BadHandlerCommandHandler : ICommandHandler {
		private readonly string data;

		public BadHandlerCommandHandler(string data) {
			this.data = data;
		}

		public int Invoke(InvocationContext context) { throw new System.NotImplementedException(); }
		public Task<int> InvokeAsync(InvocationContext context) {
			System.Console.WriteLine("This will never be called");
			return Task.FromResult(0);
		}
	}
}
