using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	[Verb("reset-events", typeof(ResetEvents))]
	public class ResetEventsOptions {
		[Option("p")]
		public string Project { get; set; } = string.Empty;

		[Option("l")]
		public string? ProjectLocation { get; set; }
	}
	public class ResetEvents : BaseHandler<ResetEventsOptions> {
		private readonly ILogger<ResetEvents> logger;

		public ResetEvents(IOptions<ResetEventsOptions> options, ILogger<ResetEvents> logger) : base(options) {
			this.logger = logger;
		}
		public override Task<int> InvokeAsync(InvocationContext context) {
			string folder;
			if (string.IsNullOrEmpty(options.ProjectLocation)) {
				folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), options.Project);
			} else {
				folder = System.IO.Path.Combine(options.ProjectLocation, options.Project);
			}
			foreach (var file in Directory.GetFiles(folder, "*.log")) {
				logger.LogInformation("Deleting log file: {file}", file);
				File.Delete(file);
			}
			return Task.FromResult(0);
		}
	}
}