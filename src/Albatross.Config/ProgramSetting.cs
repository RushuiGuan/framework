using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Albatross.Config {
	public class ProgramSetting : ConfigBase {
		public override string Key => "program";
		public const string WindowsServiceManager = "windows";
		public const string SystemDServiceManager = "systemd";

		public ProgramSetting(IConfiguration configuration) : base(configuration) {
		}

		/// <summary>
		/// systemd or windows, used by the Worker host
		/// </summary>
		public string? ServiceManager { get; set; }
		/// <summary>
		/// Required: name of the application
		/// </summary>
		public string App { get; set; } = Assembly.GetEntryAssembly()?.FullName ?? "Unknown assembly";
		/// <summary>
		/// Optional: the group of the application
		/// </summary>
		public string? Group { get; set; }
	}
}
