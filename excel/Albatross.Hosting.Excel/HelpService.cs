using Albatross.Config;
using Albatross.Excel;
using Albatross.Excel.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection;
using Albatross.Text;

namespace Albatross.Hosting.Excel {
	public record class HelpEntry {
		public string Type { get; set; } = string.Empty;
		public string Entry { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? AdditionalComments { get; set; }
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
		TableOptions helpItemsTableOptions = new TableOptionsBuilder()
			.AddColumns<HelpEntry>()
			.Build();

		private readonly EnvironmentSetting environment;
		private readonly IConfiguration configuration;
		private readonly FunctionRegistrationService functionRegistrationService;
		
		public string GetEnvironment() => $"Env = {this.environment.Value.ProperCase()}";
		public List<HelpEntry> OtherEntries { get; set; } = new List<HelpEntry>();

		public HelpService(EnvironmentSetting environment, IConfiguration configuration, FunctionRegistrationService functionRegistrationService) {
			this.environment = environment;
			this.configuration = configuration;
			this.functionRegistrationService = functionRegistrationService;
		}
		public void ShowHelp(string sheetName) {
			var sheet = My.ActiveWorkbook(sheetName).GetOrCreateSheet(sheetName);
			var list = new List<HelpEntry>();
			this.GetConfigValues(list);
			list.Add(GetAssemblyVersionHelpEntry(Assembly.GetCallingAssembly()));
			foreach (var entry in functionRegistrationService.Registrations) {
				list.Add(new HelpEntry {
					Type = "Custom Function",
					Entry = entry.FunctionAttribute.Name,
					Description = entry.FunctionAttribute.Description ?? entry.FunctionAttribute.HelpTopic,
				});
			}
			list.AddRange(OtherEntries);
			list.ToArray().WriteTable(sheet, helpItemsTableOptions);
		}
		HelpEntry GetAssemblyVersionHelpEntry(Assembly assembly) {
			var name = assembly.GetName();
			var attrib = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
			return new HelpEntry {
				Type = "AssemblyVersion",
				Entry = name.Name ?? "Anonymous",
				Description = attrib?.InformationalVersion ?? name.Version?.ToString() ?? "Unknown",
			};
		}
		void GetConfigValues(List<HelpEntry> list) {
			list.Add(new HelpEntry {
				Type = "Configuration",
				Entry = "Environment",
				Description = this.environment.Value
			});
			var section = configuration.GetSection("ConnectionStrings");
			foreach (var item in section.GetChildren()) {
				list.Add(new HelpEntry { 
					Type = "Configuration", 
					Entry = "ConnectionString", 
					Description = item.Key, 
					AdditionalComments = item.Value
				});
			}
			section = configuration.GetSection("EndPoints");
			foreach (var item in section.GetChildren()) {
				list.Add(new HelpEntry { Type = "Configuration", Entry = "EndPoint", Description = $"{item.Key}={item.Value}" });
			}
		}
	}
}
