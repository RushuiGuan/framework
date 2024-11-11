using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.DevTools {
	[Verb("build-version", typeof(BuildVersion), Description = "Bump the version number")]
	public class BuildVersionOptions {
		[Option("ver")]
		public string Version { get; set; } = string.Empty;

		[Option("ma")]
		public bool NextMajor { get; set; }
		[Option("mi")]
		public bool NextMinor { get; set; }
		[Option("pa")]
		public bool NextPatch { get; set; }

		[Option("pre")]
		public string[] Prerelease { get; set; } = new string[0];
		[Option("clear-pre")]
		public bool ClearPrerelease { get; set; }

		[Option("meta")]
		public string[] Metadata { get; set; } = new string[0];
		[Option("clear-meta")]
		public bool ClearMetadata { get; set; }
	}
	public class BuildVersion : BaseHandler<BuildVersionOptions> {
		public BuildVersion(IOptions<BuildVersionOptions> options, ILogger logger) : base(options, logger) {
		}
		public override Task<int> InvokeAsync(InvocationContext context) {
			var version = new SemVer.SematicVersion(this.options.Version);
			if (this.options.NextMajor) {
				version = version.NextRelease(SemVer.ReleaseType.Major);
			}
			if (this.options.NextMinor) {
				version = version.NextRelease(SemVer.ReleaseType.Minor);
			}
			if (this.options.NextPatch) {
				version = version.NextRelease(SemVer.ReleaseType.Patch);
			}
			if(options.ClearPrerelease) {
				version = version with { PreRelease = [] };
			}
			if (options.Prerelease.Any()) {
				version = version with {
					PreRelease = options.Prerelease
				};
			}
			if (options.ClearMetadata) {
				version = version with { Metadata = [] };
			}
			if (options.Metadata.Any()) {
				version = version with { Metadata = options.Metadata };
			}
			this.writer.WriteLine(version.ToString());
			return Task.FromResult(0);
		}
	}
}
