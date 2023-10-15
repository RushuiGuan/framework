using Albatross.Config;
using Albatross.Excel;
using Albatross.Excel.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection;
using Albatross.Text;

namespace Albatross.Hosting.Excel {
	public record class ConfigurationValue {
		public string Type { get; set; } = string.Empty;
		public string Key { get; set; } = string.Empty;
		public string? Value { get; set; }
	}
	public record class VersionValue {
		public VersionValue(AssemblyName assemblyName) {
			this.AssemblyName = assemblyName.Name ?? "Anonymous";
			this.AssemblyVersion = assemblyName.Version?.ToString() ?? "Unknown";
		}

		public string AssemblyName { get; set; } = string.Empty;
		public string AssemblyVersion { get; set; } = string.Empty;
	}
	
	public class ShowConfigService {
		public string Environment { get; set; }
		TableOptions configTableOptions = new TableOptionsBuilder()
			.AddColumns<ConfigurationValue>()
			.Build();

		TableOptions versionTableOptions = new TableOptionsBuilder()
			.Offset(0, 4)
			.AddColumns<VersionValue>()
			.Build();
		private readonly IConfiguration configuration;

		public ShowConfigService(EnvironmentSetting environment, IConfiguration configuration) {
			this.Environment = $"{environment.Value.ProperCase()} config";
			this.configuration = configuration;
		}
		public void ShowConfig(string sheetName) {
			var sheet = My.ActiveWorkbook(sheetName).GetOrCreateSheet(sheetName);
			this.GetConfigValues().WriteTable(sheet, configTableOptions);
		}
		public void ShowVerison(string sheetName, Assembly assembly) {
			var sheet = My.ActiveWorkbook(sheetName).GetOrCreateSheet(sheetName);
			GetAssemblyVersions(assembly).WriteTable(sheet, versionTableOptions);
		}
		VersionValue[] GetAssemblyVersions(Assembly assembly) {
			List<VersionValue> versions = new List<VersionValue> {
				new VersionValue(assembly.GetName())
			};
			foreach (var referenced in assembly.GetReferencedAssemblies()) {
				versions.Add(new VersionValue(referenced));
			}
			return versions.ToArray();
		}
		ConfigurationValue[] GetConfigValues() {
			var list = new List<ConfigurationValue>();
			var section = configuration.GetSection("ConnectionStrings");
			foreach (var item in section.GetChildren()) {
				list.Add(new ConfigurationValue { Type = "ConnectionString", Key = item.Key, Value = item.Value });
			}
			section = configuration.GetSection("EndPoints");
			foreach (var item in section.GetChildren()) {
				list.Add(new ConfigurationValue { Type = "EndPoint", Key = item.Key, Value = item.Value });
			}
			return list.ToArray();
		}
	}
}
