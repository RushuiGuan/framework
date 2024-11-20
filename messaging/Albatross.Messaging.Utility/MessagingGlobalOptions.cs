using Albatross.CommandLine;

namespace Albatross.Messaging.Utility {
	public record class MessagingGlobalOptions {
		[Option("a")]
		public string Application { get; set; } = string.Empty;

		[Option("f")]
		public string? EventSourceFolder { get; set; } = string.Empty;
	}
}
