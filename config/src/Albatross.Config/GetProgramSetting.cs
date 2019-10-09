using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.Config {
	public class GetProgramSetting : GetConfig<ProgramSetting> {
		IGetAssemblyLocation getAssemblyLocation;
		protected override string Key => ProgramSetting.Key;
		public override bool Required => false;

		public GetProgramSetting(IConfiguration configuration, IGetAssemblyLocation getAssemblyLocation) : base(configuration) {
			this.getAssemblyLocation = getAssemblyLocation;
		}
		
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
