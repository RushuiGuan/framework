using Albatross.CommandLine;
using Albatross.Text;
using AzureDevOpsProxy;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.DevTools.AzureDevOps {
	[Verb("azure-package-versions", typeof(GetAzurePackageVersions))]
	public record GetAzurePackageVersionsOptions {
		public string? Project { get; set; }
		[Option("n")]
		public string FeedName { get; set; } = string.Empty;
		[Option("p")]
		public string Package { get; set; } = string.Empty;
	}
	public class GetAzurePackageVersions : BaseHandler<GetAzurePackageVersionsOptions> {
		private readonly FeedManagementProxy feedManagement;
		private readonly PackageManagementProxy packageManagement;

		public GetAzurePackageVersions(FeedManagementProxy feedManagement, PackageManagementProxy packageManagement, IOptions<GetAzurePackageVersionsOptions> options) : base(options) {
			this.feedManagement = feedManagement;
			this.packageManagement = packageManagement;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var feeds = await feedManagement.GetFeeds(options.Project);
			var feed = feeds.FirstOrDefault(x => x.Name.ToLowerInvariant() == options.FeedName.ToLowerInvariant())
				?? throw new System.Exception($"Feed {options.FeedName} not found");

			var packages = await packageManagement.GetPackages(options.Project, feed.Id);
			var package = packages.FirstOrDefault(x => x.Name.ToLowerInvariant() == options.Package.ToLowerInvariant())
				?? throw new System.Exception($"Package {options.Package} not found");


			//var text = await packageManagement.GetPackageVersionsRaw(options.Project, options.FeedName, package.Id);
			//this.writer.WriteLine(text);

			var versions = await packageManagement.GetPackageVersions(options.Project, options.FeedName, package.Id);
			await this.writer.PrintTable(versions, new PrintOptionBuilder<PrintTableOption>()
				.Property("Id", "Version", "IsLatest", "IsListed", "PublishDate")
				.Format("PublishDate", "yyyy-MM-ddTHH:mmzzz")
				.Build());
			return 0;
		}
	}
}
