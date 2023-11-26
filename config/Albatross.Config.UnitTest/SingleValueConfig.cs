using Microsoft.Extensions.Configuration;

namespace Albatross.Config.UnitTest {
	public class SingleValueConfig : ConfigBase {
		public override string Key => "single-value-config";
		public string? Value{ get; set; }
		public SingleValueConfig(IConfiguration configuration) : base(configuration) {
			this.Value = configuration.GetSection(Key).Get<string>();
		}
	}
}
