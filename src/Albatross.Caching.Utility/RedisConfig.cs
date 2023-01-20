using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Caching.Utility {
	public class RedisConfig : ConfigBase {
		public RedisConfig(IConfiguration configuration) : base(configuration) {
			this.RedisConnectionString = configuration.GetRequiredConnectionString("redis");
		}
		public string RedisConnectionString { get; set; }
		public string User { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
