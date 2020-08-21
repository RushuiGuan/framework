using Microsoft.Extensions.Configuration;

namespace Albatross.Config.UnitTest {
	public class EmptyConfig { }

	public class GetEmptyConfig: GetConfig<EmptyConfig> {
		public GetEmptyConfig(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => "empty-config";
	}
}