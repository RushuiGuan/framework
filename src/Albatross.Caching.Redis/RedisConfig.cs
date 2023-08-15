using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Caching.Redis {
	public enum RedisValueFormat {
		String = 0,
		Bytes = 1,
		Json = 2
	}
	public class RedisConfig : ConfigBase {
		public override string Key => "redis";
		public RedisConfig(IConfiguration configuration) : base(configuration) {
			RedisConnectionString = configuration.GetRequiredEndPoint("redis", false);
			if (string.IsNullOrEmpty(InstanceName)) {
				throw new ConfigurationException("redis:instanceName");
			}
			if (string.IsNullOrEmpty(User)) {
				throw new ConfigurationException("redis:user");
			}
			if (string.IsNullOrEmpty(Password)) {
				throw new ConfigurationException("redis:password");
			}
			if (!InstanceName.EndsWith(ICacheManagement.CacheKeyDelimiter)) {
				InstanceName += ICacheManagement.CacheKeyDelimiter;
			}
		}
		public string RedisConnectionString { get; set; } = null!;
		public string InstanceName { get; set; } = null!;
		public string User { get; set; } = null!;
		public string Password { get; set; } = null!;
		public RedisValueFormat RedisValueFormat { get; set; }
	}
}
