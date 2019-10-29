using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace Albatross.Config {
	public class SetupConfig {
		private readonly string BasePath;
		private readonly string environmentPrefix;

		public IConfiguration Configuration { get; private set; }
		public ProgramSetting ProgramSetting => this.Configuration.GetValue<ProgramSetting>(ProgramSetting.Key);

		public SetupConfig(string basePath = null, string environmentPrefix = null) {
			this.BasePath = basePath ?? Directory.GetCurrentDirectory();
			this.environmentPrefix = environmentPrefix;
			Run();
		}

		private void Run() {
			var builder = new ConfigurationBuilder();
			builder.SetBasePath(this.BasePath);
			builder.AddJsonFile("hostsettings.json", optional: true);
			builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			builder.AddEnvironmentVariables(this.environmentPrefix);
			this.Configuration = builder.Build();
		}
	}
}
