using System.Collections.Generic;
using System.Linq;
using System;

namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReportingResult {
		public bool HasChange => Changes.Any();
		public IEnumerable<IChangeReport> Changes { get; set; } = Array.Empty<IChangeReport>();
		public string Text { get; set; } = string.Empty;
	}
}