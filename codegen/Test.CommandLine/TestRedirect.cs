using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using Test.Proxy;

namespace Test.CommandLine {
	[Verb("test-redirect", typeof(TestRedirectCommandHandler))]
	public class TestRedirectOptions {
		public bool AbsUrl { get; set; }
		public int ActionId { get; set; }
	}

	public class TestRedirectCommandHandler : ICommandHandler {
		private readonly RedirectTestProxyService client;
		private readonly AbsUrlRedirectTestProxyService absClient;
		private readonly TestRedirectOptions options;

		public TestRedirectCommandHandler(IOptions<TestRedirectOptions> options, RedirectTestProxyService client, AbsUrlRedirectTestProxyService absClient) {
			this.client = client;
			this.absClient = absClient;
			this.options = options.Value;
			client.UseTextWriter(System.Console.Out);
		}

		public int Invoke(InvocationContext context) {
			throw new NotImplementedException();
		}

		public async Task<int> InvokeAsync(InvocationContext context) {
			if (options.AbsUrl) {
				await absClient.Get(options.ActionId);
			} else {
				await client.Get(options.ActionId);
			}
			return 0;
		}
	}
}