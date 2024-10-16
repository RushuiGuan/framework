using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sample.CommandLine {
	[Verb("backup", typeof(BackupCommandHandler))]
	public class BackupCommandOptions {
	}
	[Verb("back-me-up", typeof(BackupCommandHandler))]
	public class BackupOptions {
	}
	public class BackupCommandHandler : BaseHandler<BackupCommandOptions> {
		public BackupCommandHandler(IOptions<BackupCommandOptions> options, ILogger logger) : base(options, logger) {
		}
	}
}
