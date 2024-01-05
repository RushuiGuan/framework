using System.Collections.Generic;
using System.Linq;
using System;

namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReportingResult {
		public bool HasAnyChanges { get; set;  }
		public bool HasReportedChanges => Changes.Any();
		public List<IChangeReport> Changes { get; } = new List<IChangeReport>();
		public string Text { get; set; } = string.Empty;
	}
}