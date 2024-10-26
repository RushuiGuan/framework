using Albatross.CommandLine;
using System.CommandLine.Invocation;
using Test.Dto.Classes;
using Test.Proxy;

namespace Test.CommandLine {
	[Verb("test", typeof(TestCommandHandler))]
	public class TestOptions {
	}
	public class TestCommandHandler : ICommandHandler {
		private readonly FromBodyParamTestProxyService proxy;

		public TestCommandHandler(FromBodyParamTestProxyService proxy) {
			this.proxy = proxy;
		}

		public int Invoke(InvocationContext context) {
			throw new NotImplementedException();
		}

		public async Task<int> InvokeAsync(InvocationContext context) {
			await proxy.RequiredObject(new MyDto());
			return 0;
		}
	}
}