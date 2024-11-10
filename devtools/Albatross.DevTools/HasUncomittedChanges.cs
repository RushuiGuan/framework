using Albatross.CommandLine;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.DevTools {
	[Verb("is-dirty", typeof(HasUncomittedChanges), Description = "Check if the directory has uncomitted changes")]
	public class HasUncomittedChangesOptions {
		[Option("d")]
		public DirectoryInfo Directory { get; set; } = null!;
		[Option("s")]
		public bool Show { get; set; }
	}
	public class HasUncomittedChanges : BaseHandler<HasUncomittedChangesOptions> {
		public HasUncomittedChanges(IOptions<HasUncomittedChangesOptions> options, ILogger logger) : base(options, logger) {
		}
		public override Task<int> InvokeAsync(InvocationContext context) {
			var gitDirectory = Repository.Discover(options.Directory.FullName);
			if (gitDirectory != null) {
				var rootDirectory = new DirectoryInfo(gitDirectory).Parent
					?? throw new System.Exception("Parent directory of Git Directory is not found");
				using var repo = new Repository(gitDirectory);
				var relativePath = Path.GetRelativePath(rootDirectory.FullName, options.Directory.FullName);
				var status = repo.RetrieveStatus();
				var isDirty = false;
				foreach (var entry in status) {
					if (entry.State != FileStatus.Ignored && entry.FilePath.StartsWith(relativePath + "/")) {
						isDirty = true;
						if (options.Show) {
							this.writer.WriteLine($"{entry.State}: {entry.FilePath}");
						}
					}
				}
				if (isDirty) {
					return Task.FromResult(1);
				} else {
					return Task.FromResult(0);
				}
			} else {
				logger.LogError("git directory not found");
				return Task.FromResult(1);
			}
		}
	}
}
