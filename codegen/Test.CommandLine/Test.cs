using Albatross.CommandLine;
using System.CommandLine.Invocation;
using Test.Proxy;

namespace Test.CommandLine {
	[Verb("test", typeof(TestCommandHandler))]
	public class TestOptions {
	}
	public class TestCommandHandler : ICommandHandler {
		private readonly FromQueryParamTestProxyService proxy;

		public TestCommandHandler(FromQueryParamTestProxyService proxy) {
			this.proxy = proxy;
		}

		public int Invoke(InvocationContext context) {
			throw new NotImplementedException();
		}

		public async Task<int> InvokeAsync(InvocationContext context) {
			await proxy.RequiredString("test");
			return 0;
		}
	}
}
