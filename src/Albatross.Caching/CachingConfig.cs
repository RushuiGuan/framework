using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Caching {
	public class CachingConfig : ConfigBase {
		public override string Key => "caching";

		public CachingConfig(IConfiguration configuration) : base(configuration) {
		}

		public string[] SiblingEndPoints { get; set; } = new string[0];
	}
}
