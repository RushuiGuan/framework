using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;

namespace Albatross.Config {
	public class MyHostEnvironment : IHostEnvironment {
		private readonly ProgramSetting programSetting;
		private readonly EnvironmentSetting environmentSetting;

		public MyHostEnvironment(ProgramSetting programSetting, EnvironmentSetting environmentSetting) {
			this.programSetting = programSetting;
			this.environmentSetting = environmentSetting;
		}

		public string EnvironmentName { get => this.environmentSetting.Value; set { } }
		public string ApplicationName { get => this.programSetting.App; set { } }
		public string ContentRootPath { get => AppContext.BaseDirectory; set { } }
		IFileProvider fileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
		public IFileProvider ContentRootFileProvider { get => fileProvider; set { } }
	}
}
