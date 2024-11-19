using Albatross.CommandLine;
using Albatross.Text;
using AzureDevOpsProxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albatross.DevTools.AzureDevOps {
	[Verb("azure-packages", typeof(GetAzurePackages))]
	public record GetAzurePackagesOptions {
		[Option("p")]
		public string? Project { get; set; }
		
		[Option("f")]
		public string Feed { get; set; } = string.Empty;

		[Option("pattern")]
		public string? Pattern { get; set; }
	}
	public class GetAzurePackages : BaseHandler<GetAzurePackagesOptions> {
		private readonly FeedManagementProxy feedManagement;
		private readonly PackageManagementProxy packageManagement;

		public GetAzurePackages(FeedManagementProxy feedManagement, PackageManagementProxy packageManagement, IOptions<GetAzurePackagesOptions> options, ILogger logger) : base(options, logger) {
			this.feedManagement = feedManagement;
			this.packageManagement = packageManagement;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var feeds = await feedManagement.GetFeeds(options.Project);
			var feed = feeds.FirstOrDefault(x => x.Name.ToLowerInvariant() == options.Feed.ToLowerInvariant())
				?? throw new System.Exception($"Feed {options.Feed} not found");

			var packages = await packageManagement.GetPackages(options.Project, feed.Id);
			if (!string.IsNullOrEmpty(options.Pattern)) {
				var regex = new Regex(options.Pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
				packages = packages.Where(x => regex.IsMatch(x.Name)).ToArray();
			}
			await writer.PrintTable(packages, new PrintOptionBuilder<PrintTableOption>().Property("Id", "Name").Build());
			return 0;
		}
	}
}
