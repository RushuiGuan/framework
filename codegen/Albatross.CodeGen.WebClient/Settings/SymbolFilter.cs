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

		public bool ShouldKeep(string name) {
			if (this.exclusiveFilter != null && this.exclusiveFilter.IsMatch(name)) {
				return false;
			}
			if (this.inclusiveFilter != null && !this.inclusiveFilter.IsMatch(name)) {
				return false;
			}
			return true;
		}
	}
}
