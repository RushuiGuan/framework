using Albatross.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReportingOptions {
		public List<string> FixedHeaders { get; set; } = new List<string>();
		public ChangeType Type { get; set; } = ChangeType.Modified;
		public HashSet<string> SkippedProperties { get; } = new HashSet<string>();
		public string? Prefix { get; set; }
		public string? Postfix { get; set; }
		public IReadOnlyDictionary<string, Func<int, Task<string>>> Lookups { get; set; } = new Dictionary<string, Func<int, Task<string>>>();
	}
}