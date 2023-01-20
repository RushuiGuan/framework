using Albatross.Config;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Caching {
	public class CachingConfig : ConfigBase {
		public override string Key => "caching";
		public CachingConfig(IConfiguration configuration) : base(configuration) {
			this.RedisConnectionString = configuration.GetRequiredConnectionString("redis");
		}
		public string RedisConnectionString { get; set; }
		[Required]
		public string InstanceName { get; set; } = null!;
		[Required]
		public string User { get; set; } = null!;
		[Required]
		public string Password { get; set; } = null!;
	}
}
