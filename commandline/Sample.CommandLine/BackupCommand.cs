using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sample.CommandLine {
	[Verb("backup", typeof(BackupCommandHandler))]
	public class BackupCommandOptions {
		[Option("f", "file", "---file-name", Description = "The name of the file to backup")]
		public string? FileName { get; set; }
	}
	[Verb("back-me-up", typeof(BackMeUpCommandHandler))]
	public class BackupOptions {
		public string? DirectoryName { get; set; }
	}
	public class BackupCommandHandler : BaseHandler<BackupCommandOptions> {
		public BackupCommandHandler(IOptions<BackupCommandOptions> options, ILogger logger) : base(options, logger) {
		}
	}
	public class BackMeUpCommandHandler : BaseHandler<BackupOptions> {
		public BackMeUpCommandHandler(IOptions<BackupOptions> options, ILogger logger) : base(options, logger) {
		}
	}
}