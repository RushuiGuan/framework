﻿using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Albatross.Config {
	public class GetProgramSetting : GetConfig<ProgramSetting> {
		protected override string Key => ProgramSetting.Key;

		public GetProgramSetting(IConfiguration configuration) : base(configuration) {
		}
		
		public override ProgramSetting Get() {
			ProgramSetting setting = base.Get() ?? new ProgramSetting();
			if (string.IsNullOrEmpty(setting.App)) {
				setting.App = Assembly.GetEntryAssembly().FullName;
			}
			return setting;
		}
	}
}
