using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Repository.UnitTest {
	public class GetTestSettings : GetConfig<TestSetting> {
		public GetTestSettings(IConfiguration configuration) : base(configuration) { }
		protected override string Key => TestSetting.Key;
	}
}
