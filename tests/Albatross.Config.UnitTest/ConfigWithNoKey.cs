using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Config.UnitTest {
	public class ConfigWithNoKey : ConfigBase {
		public ConfigWithNoKey(IConfiguration configuration) : base(configuration) {
			this.ConnectionString = configuration.GetConnectionString("my-database");
			this.EndPoint = configuration.GetEndPoint("my-api") ?? throw new ValidationException("Missing my-api");
		}

		public string ConnectionString { get; }
		public string EndPoint{ get; }
	}
}