using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("post", typeof(PostCommandHandler), UseBaseClassProperties = true, Description = "Create a new item")]
	public record class PostCommandOptions : BaseCommandOptions {
		public string Value { get; set; } = string.Empty;
	}

	public class PostCommandHandler : BaseHandler<PostCommandOptions> {
		public PostCommandHandler(IOptions<PostCommandOptions> options, ILogger logger) : base(options, logger) {
		}

		public override Task<int> InvokeAsync(InvocationContext context) {
			throw new NotImplementedException();
		}
	}
}
