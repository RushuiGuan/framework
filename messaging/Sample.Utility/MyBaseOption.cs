using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Proxy;

namespace Sample.Utility {
	public class MyBaseOption {
		[Option("c")]
		public int Count { get; set; } = 10;
	}

	public class MyBaseHandler<T> : BaseHandler<T> where T : class {
		protected readonly CommandProxyService commandProxy;
		protected readonly ILogger logger;

		public MyBaseHandler(CommandProxyService commandProxy, IOptions<T> options, ILogger logger) : base(options) {
			this.commandProxy = commandProxy;
			this.logger = logger;
		}
	}
}