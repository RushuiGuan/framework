using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.Config {
	public class GetProgramSetting : GetConfig<ProgramSetting> {
		IGetAssemblyLocation getAssemblyLocation;

		public GetProgramSetting(IGetConfigValue getConfigValue, IGetAssemblyLocation getAssemblyLocation) : base(getConfigValue) {
			this.getAssemblyLocation = getAssemblyLocation;
		}

		protected override string Key => ProgramSetting.Key;
		public override bool Required => false;
		public override ProgramSetting Get() {
			ProgramSetting setting = base.Get() ?? new ProgramSetting();
			if (string.IsNullOrEmpty(setting.App)) {
				setting.App = Path.GetFileName(getAssemblyLocation.CodeBase);
			}
			if (string.IsNullOrEmpty(setting.Environment)) {
				setting.Environment = "dev";
			}
			return setting;
		}
	}
}
