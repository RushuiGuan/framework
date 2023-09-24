using Albatross.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReportingOptions {
		public ChangeReportingOptions() {
			FormatValueFunc = DefaultFormatValue;
		}
		public IEnumerable<string> Properties { get; set; } = Array.Empty<string>();
		public ChangeType Type { get; set; } = ChangeType.Modified;
		public PrintOption.FormatValueDelegate? FormatValueFunc { get; set; }
		public string[] SkippedProperties { get; set; } = new string[] { "PeriodStart", "PeriodEnd" };
		public string? Prefix { get; set; }
		public string? Postfix { get; set; }
		public IReadOnlyDictionary<string, Func<int, Task<string>>> Lookups { get; set; } = new Dictionary<string, Func<int, Task<string>>>();
		public bool DoNotSave { get; set; }
		public async Task<string> DefaultFormatValue(object? entity, string property, object? value) {
			if (entity == null) {
				return string.Empty;
			} else {
				try {
					var changeReport = (IChangeReport)entity;
					if ((property == nameof(IChangeReport.OriginalValue) || property == nameof(IChangeReport.CurrentValue)) && Lookups.TryGetValue(changeReport.Property, out var func)) {
						var id = Convert.ToInt32(value);
						return await func(id);
					}
				} catch { 
				}
				if (value is DateOnly || value is DateTime dateTime && dateTime.TimeOfDay == TimeSpan.Zero) {
					return $"{value:yyyy-MM-dd}";
				} else if (value is DateTime) {
					return $"{value:yyyy-MM-dd HH:mm:ssz}";
				} else {
					return Convert.ToString(value) ?? string.Empty;
				}
			}
		}
	}
}