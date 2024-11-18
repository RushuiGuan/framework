using Albatross.Config;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AzureDevOpsProxy {
	public class AzureDevOpsProxyConfig : ConfigBase {
		public override string Key => "azureDevOps";
		public AzureDevOpsProxyConfig(IConfiguration configuration) : base(configuration) {
			this.FeedsEndPoint = configuration.GetRequiredEndPoint("azureDevOps_feeds");
			this.PackagesEndPoint = configuration.GetRequiredEndPoint("azureDevOps_packages");
		}
		[Required]
		public string Organization { get; set; } = string.Empty;
		public string FeedsEndPoint { get; set; }
		public string PackagesEndPoint { get; set; }
		public string ApiVersion { get; set; } = "7.1";
		[Required]
		public string PAT { get; set; } = string.Empty;

		public string FeedsBaseUrl => $"{FeedsEndPoint}{Organization}/";
		public string PackagesBaseUrl => $"{PackagesEndPoint}{Organization}/";
		public string BasicAuth => Convert.ToBase64String(Encoding.ASCII.GetBytes($":{PAT}"));
	}
}
