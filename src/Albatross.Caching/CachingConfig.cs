using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Caching {
	public enum RedisValueFormat {
		String = 0, 
		Bytes = 1, 
		Json = 2
	}
	public class CachingConfig : ConfigBase {
		public override string Key => "caching";
		public CachingConfig(IConfiguration configuration) : base(configuration) {
			if (Distributed) {
				this.RedisConnectionString = configuration.GetRequiredConnectionString("redis");
				if(!string.IsNullOrEmpty(this.InstanceName)) {
					throw new ConfigurationException("caching:instanceName");
				}
				if(!string.IsNullOrEmpty(this.User)) {
					throw new ConfigurationException("caching:user");
				}
				if(!string.IsNullOrEmpty(this.Password)) {
					throw new ConfigurationException("caching:password");
				}
			}
		}
		public string RedisConnectionString { get; set; } = null!;
		public string InstanceName { get; set; } = null!;
		public string User { get; set; } = null!;
		public string Password { get; set; } = null!;
		public bool Distributed { get; set; }
		public RedisValueFormat RedisValueFormat { get; set; }
	}
}
