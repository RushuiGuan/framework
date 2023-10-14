using Albatross.Config;
using Albatross.Excel;
using Albatross.Excel.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Albatross.Hosting.Excel {
	public record ConfigurationValue {
		public string Type { get; set; } = string.Empty;
		public string Key { get; set; } = string.Empty;
		public string? Value { get; set; }
	}
	public class ShowConfigService {
		public string Environment { get; set; }
		public ConfigurationValue[] Values { get; } = Array.Empty<ConfigurationValue>();
		TableOptions configTableOptions = new TableOptionsBuilder()
			.AddColumns<ConfigurationValue>()
			.Build();

		public ShowConfigService(EnvironmentSetting environment, IConfiguration configuration) {
			this.Environment = environment.Value;
			var list = new List<ConfigurationValue>();
			var section = configuration.GetSection("ConnectionStrings");
			foreach (var item in section.GetChildren()) {
				list.Add(new ConfigurationValue { Type = "ConnectionString", Key = item.Key, Value = item.Value });
			}
			section = configuration.GetSection("EndPoints");
			foreach (var item in section.GetChildren()) {
				list.Add(new ConfigurationValue { Type = "EndPoint", Key = item.Key, Value = item.Value });
			}
			Values = list.ToArray();
		}
		public void ShowConfig(string appName) {
			var sheetName = $"{appName}_config";
			var sheet = My.ActiveWorkbook(sheetName).GetOrCreateSheet(sheetName);
			Values.WriteTable(sheet, configTableOptions);
		}
	}
}
