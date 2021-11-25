using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;

namespace Albatross.Config {
	public class GetProgramSetting : GetConfig<ProgramSetting> {
		protected override string Key => ProgramSetting.Key;

		public GetProgramSetting(IConfiguration configuration) : base(configuration) {
		}
		
		public override ProgramSetting Get() {
			ProgramSetting setting = base.Get() ?? new ProgramSetting();
			return setting;
		}
	}
}
