using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient.Settings {
	public class SymbolFilter {
		Regex? inclusiveFilter, exclusiveFilter;

		public SymbolFilter(SymbolFilterPatterns patterns) {
			if (patterns.Include != null) {
				this.inclusiveFilter = new Regex(patterns.Include, RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
			if (patterns.Exclude != null) {
				this.exclusiveFilter = new Regex(patterns.Exclude, RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
		}

		public bool Exclude(string name) {
			return this.exclusiveFilter != null && this.exclusiveFilter.IsMatch(name);
		}
		public bool Include(string name) {
			return this.inclusiveFilter != null && this.inclusiveFilter.IsMatch(name);
		}

		/// <summary>
		/// If there is no match by include or exclude filter, method will return true.  If both include and exclude filter are matched, then include wins
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool ShouldKeep(string name) {
			return !Exclude(name) || Include(name) || this.exclusiveFilter == null && this.inclusiveFilter == null;
		}

		/// <summary>
		/// Implement the same behavior with the single pattern filter.  Include wins over exclude
		/// </summary>
		/// <param name="filters"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool ShouldKeep(IEnumerable<SymbolFilter> filters, string name) {
			bool excluded = false;
			foreach (var filter in filters) {
				if (filter.Include(name)) {
					return true;
				} else {
					excluded = excluded || filter.Exclude(name);
				}
			}
			return !excluded;
		}
	}
}