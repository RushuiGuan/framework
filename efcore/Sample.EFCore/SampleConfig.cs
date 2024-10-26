using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Sample.EFCore {
	public class SampleConfig : ConfigBase {
		public SampleConfig(IConfiguration configuration) : base(configuration) {
			this.ConnectionString = configuration.GetRequiredConnectionString("sample");
		}
		public string ConnectionString { get; set; }
	}
}