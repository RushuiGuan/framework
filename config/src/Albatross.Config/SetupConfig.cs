using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Albatross.Config {
	public class SetupConfig {
		const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
		public const string Default_Environment_Prefix = "ASPNETCORE_";
		public string Environment => System.Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT);
		private readonly bool useHostSetting;
		private readonly string BasePath;
		private readonly string environmentPrefix;

		public IConfiguration Configuration { get; private set; }
		public ProgramSetting ProgramSetting => this.Configuration.GetValue<ProgramSetting>(ProgramSetting.Key);

		public SetupConfig(string basePath, string environmentPrefix = Default_Environment_Prefix, bool useHostSetting = true) {
			this.useHostSetting = useHostSetting;
			this.BasePath = basePath;
			this.environmentPrefix = environmentPrefix;
			Run();
		}

		private void Run() {
			var builder = new ConfigurationBuilder();
			builder.SetBasePath(this.BasePath);
			if (this.useHostSetting) {
				builder.AddJsonFile("hostsettings.json", optional: true);
				if (!string.IsNullOrEmpty(Environment)) {
					builder.AddJsonFile($"hostsettings.{Environment}.json", optional: true);
				}
			}
			builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
			builder.AddEnvironmentVariables(this.environmentPrefix);
			this.Configuration = builder.Build();
		}
	}
}
