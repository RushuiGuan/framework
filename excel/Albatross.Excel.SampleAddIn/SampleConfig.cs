using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Excel.SampleAddIn {
	public class SampleConfig : ConfigBase {
		public SampleConfig(IConfiguration configuration) : base(configuration) {
			this.ConnectionString = configuration.GetRequiredConnectionString("db");
		}
		public string ConnectionString { get; set; }
	}
}