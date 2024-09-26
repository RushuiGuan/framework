using Albatross.CommandLine;
using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("bad-handler", typeof(BadHandlerCommandHandler))]
	public class BadHandlerCommandOptions {
	}
	public class BadHandlerCommandHandler : ICommandHandler{
		public BadHandlerCommandHandler(string input) {
		}

		public int Invoke(InvocationContext context) {
			throw new NotImplementedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			throw new NotImplementedException();
		}
	}
}
