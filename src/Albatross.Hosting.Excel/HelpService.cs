using Albatross.Config;
using Albatross.Excel;
using Albatross.Excel.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection;
using Albatross.Text;
using System.Diagnostics.Contracts;

namespace Albatross.Hosting.Excel {
	public record class HelpEntry {
		public string Type { get; set; } = string.Empty;
		public string Key { get; set; } = string.Empty;
		public string? Value { get; set; }
	}
	public record class VersionValue {
		public VersionValue(AssemblyName assemblyName, AssemblyInformationalVersionAttribute? informationalVersionAttribute) {
			this.AssemblyName = assemblyName.Name ?? "Anonymous";
			this.AssemblyVersion = informationalVersionAttribute?.InformationalVersion ?? assemblyName.Version?.ToString() ?? "Unknown";
		}

		public string AssemblyName { get; set; } = string.Empty;
		public string AssemblyVersion { get; set; } = string.Empty;
	}
	
	public class HelpService {
		public string Environment { get; set; }
		TableOptions helpItemsTableOptions = new TableOptionsBuilder()
			.AddColumns<HelpEntry>()
			.Build();

		private readonly IConfiguration configuration;
		private readonly FunctionRegistrationService functionRegistrationService;

		public HelpService(EnvironmentSetting environment, IConfiguration configuration, FunctionRegistrationService functionRegistrationService) {
			this.Environment = $"Env = {environment.Value.ProperCase()}";
			this.configuration = configuration;
			this.functionRegistrationService = functionRegistrationService;
		}
		public void ShowHelp(string sheetName) {
			var sheet = My.ActiveWorkbook(sheetName).GetOrCreateSheet(sheetName);
			var list = new List<HelpEntry>();
			this.GetConfigValues(list);
			list.Add(GetAssemblyVersionHelpEntry(Assembly.GetCallingAssembly()));
			foreach(var entry in functionRegistrationService.Registrations) {
				list.Add(new HelpEntry { 
					Type = "Custom Function",
					Key = entry.FunctionAttribute.Name,
					Value = entry.FunctionAttribute.Description ?? entry.FunctionAttribute.HelpTopic,
				});
			}
			list.ToArray().WriteTable(sheet, helpItemsTableOptions);
		}
		HelpEntry GetAssemblyVersionHelpEntry(Assembly assembly) {
			var name = assembly.GetName();
			var attrib = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
			return new HelpEntry {
				Type = "AssemblyVersion",
				Key = name.Name ?? "Anonymous",
				Value = attrib?.InformationalVersion ?? name.Version?.ToString() ?? "Unknown",
			};
		}
		void GetConfigValues(List<HelpEntry> list) {
			var section = configuration.GetSection("ConnectionStrings");
			foreach (var item in section.GetChildren()) {
				list.Add(new HelpEntry { Type = "ConnectionString", Key = item.Key, Value = item.Value });
			}
			section = configuration.GetSection("EndPoints");
			foreach (var item in section.GetChildren()) {
				list.Add(new HelpEntry { Type = "EndPoint", Key = item.Key, Value = item.Value });
			}
		}
	}
}
