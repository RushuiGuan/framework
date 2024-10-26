using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.WebClient.Test {
	public class MyConfig : ConfigBase {
		public MyConfig(IConfiguration configuration) : base(configuration) {
			this.TestUrl = configuration.GetRequiredEndPoint("test");
			this.Test1Url = configuration.GetRequiredEndPoint("test1");
			this.ProjectTemplateUrl = configuration.GetRequiredEndPoint("mytemplate");
		}
		public string TestUrl { get; private set; }
		public string Test1Url { get; private set; }
		public string ProjectTemplateUrl { get; }
	}
}