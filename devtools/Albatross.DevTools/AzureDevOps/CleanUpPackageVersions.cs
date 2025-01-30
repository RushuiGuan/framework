using Albatross.CommandLine;
using Albatross.Text;
using AzureDevOpsProxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albatross.DevTools.AzureDevOps {
	[Verb("cleanup-package-versions", typeof(CleanUpPackageVersions))]
	public class CleanUpPackageVersionsOptions {
		public string? Project { get; set; }
		[Option("f")]
		public string Feed { get; set; } = string.Empty;
		public bool Execute { get; set; }
		[Option("p")]
		public string PackagePattern { get; set; } = string.Empty;
		[Option("prerelease")]
		public bool PrereleaseOnly { get; set; } = true;
		public string Start { get; set; } = string.Empty;
		public string End { get; set; } = string.Empty;
	}
	public class CleanUpPackageVersions : BaseHandler<CleanUpPackageVersionsOptions> {
		private readonly FeedManagementProxy feedManagement;
		private readonly PackageManagementProxy packageManagement;
		private readonly PackageOperationProxy packageOperation;
		private readonly ILogger<CleanUpPackageVersions> logger;

		public CleanUpPackageVersions(FeedManagementProxy feedManagement, PackageManagementProxy packageManagement, PackageOperationProxy packageOperation, IOptions<CleanUpPackageVersionsOptions> options, ILogger<CleanUpPackageVersions> logger) : base(options) {
			this.feedManagement = feedManagement;
			this.packageManagement = packageManagement;
			this.packageOperation = packageOperation;
			this.logger = logger;
		}
		public class Candidate {
			public Candidate(Feed feed, Package package, PackageVersion version) {
				Feed = feed;
				Package = package;
				Version = version;
			}

			public Feed Feed { get; set; }
			public Package Package { get; set; }
			public PackageVersion Version { get; set; }
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			this.logger.LogInformation("{@options}", this.options);
			var feeds = await feedManagement.GetFeeds(options.Project);
			var feed = feeds.FirstOrDefault(x => x.Name.ToLowerInvariant() == options.Feed.ToLowerInvariant())
				?? throw new System.Exception($"Feed {options.Feed} not found");

			var packages = await packageManagement.GetPackages(options.Project, feed.Id);
			if (!string.IsNullOrEmpty(options.PackagePattern)) {
				var regex = new Regex(options.PackagePattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
				packages = packages.Where(x => regex.IsMatch(x.Name)).ToArray();
			}
			var candidates = new List<Candidate>();
			foreach (var package in packages) {
				var versions = await packageManagement.GetPackageVersions(options.Project, feed.Id, package.Id);
				var startVersion = new SemVer.SematicVersion(options.Start);
				var endVersion = new SemVer.SematicVersion(options.End);
				foreach (var version in versions.Where(x => {
					var version = new SemVer.SematicVersion(x.Version);
					return startVersion.CompareTo(version) <= 0 && version.CompareTo(endVersion) <= 0 && (!options.PrereleaseOnly || !version.IsRelease);
				})) {
					candidates.Add(new Candidate(feed, package, version));
				}
			}

			await writer.PrintTable(candidates, new PrintOptionBuilder<PrintTableOption>()
				.Property("Package.Name", "Version.Version")
				.ColumnHeader(x => x
					switch {
						"Package.Name" => "Package",
						"Version.Version" => "Version",
						_ => x
					}
				).Build());

			if (options.Execute) {
				foreach (var candidate in candidates) {
					await packageOperation.DeleteNugetPackageVersion(candidate.Feed.Id, candidate.Package.Name, candidate.Version.Version);
				}
			}
			return 0;
		}
	}
}