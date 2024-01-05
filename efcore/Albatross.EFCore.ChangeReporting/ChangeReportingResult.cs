using System.Collections.Generic;
using System.Linq;
using System;

namespace Albatross.EFCore.ChangeReporting {
	public class ChangeReportingResult<T> where T : class {
		public bool HasReportedChanges => Changes.Any();
		public List<T> ChangedEntities { get; } = new List<T>();
		public List<ChangeReport<T>> Changes { get; } = new List<ChangeReport<T>>();
		public string Text { get; set; } = string.Empty;
	}
}