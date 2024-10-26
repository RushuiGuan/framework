using Microsoft.Extensions.Configuration;

namespace Albatross.Config.UnitTest {
	public class NoLongerRequiredConfig : ConfigBase {
		public NoLongerRequiredConfig(IConfiguration configuration) : base(configuration) {
		}
		public override string Key => "important";
	}
}