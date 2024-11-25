using Albatross.CommandLine;
using Microsoft.Extensions.Options;

namespace Sample.CommandLine {
	[Verb("post", typeof(PostCommandHandler), UseBaseClassProperties = true, Description = "Create a new item")]
	public record class PostCommandOptions : BaseCommandOptions {
		public string Value { get; set; } = string.Empty;
	}

	public class PostCommandHandler : BaseHandler<PostCommandOptions> {
		public PostCommandHandler(IOptions<PostCommandOptions> options) : base(options) {
		}
	}
}