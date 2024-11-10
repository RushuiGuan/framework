using Albatross.CommandLine;
using Albatross.SemVer;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.DevTools {
	[Verb("project-version", typeof(BuildProjectVersion))]
	public record class BuildProjectVersionOptions {
		[Option("d")]
		public DirectoryInfo Directory { get; set; } = null!;
		[Option("p")]
		public bool Prod { get; set; }
	}
	public class BuildProjectVersion : BaseHandler<BuildProjectVersionOptions> {
		public BuildProjectVersion(IOptions<BuildProjectVersionOptions> options, ILogger logger) : base(options, logger) { }

		private string GetVersionFromFile() {
			var versionFile = Path.Join(this.options.Directory.FullName, ".version");
			if (File.Exists(versionFile)) {
				var text = File.ReadAllText(versionFile);
				return text.Trim();
			} else {
				throw new Exception($".version file is not found at {this.options.Directory.FullName}");
			}
		}
		public override Task<int> InvokeAsync(InvocationContext context) {
			var versionText = GetVersionFromFile();

			var gitDirectory = Repository.Discover(options.Directory.FullName);
			if (gitDirectory != null) {
				using var repo = new Repository(gitDirectory);
				var branch = repo.Head.FriendlyName;
				var commitCount = repo.Commits.Count();
				var hash = repo.Head.Tip.Sha.Substring(0, 7);
				SematicVersion semver;
				if (options.Prod) {
					semver = new SematicVersion(versionText) {
						Metadata = [
							hash
						]
					};
				} else {
					semver = new SematicVersion(versionText) {
						PreRelease = [
							$"{commitCount}",
							branch,
						],
						Metadata = [
							hash
						]
					};
				}
				this.writer.WriteLine(semver.ToString());
				return Task.FromResult(0);
			} else {
				logger.LogError("git directory not found");
				return Task.FromResult(1);
			}
		}
	}
}
