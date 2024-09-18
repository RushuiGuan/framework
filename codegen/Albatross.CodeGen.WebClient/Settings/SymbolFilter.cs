using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient.Settings {
	public class SymbolFilter {
		Regex? inclusiveFilter, exclusiveFilter;

		public SymbolFilter(SymbolFilterPatterns patterns) {
			if (patterns.Inclusion != null) {
				this.inclusiveFilter = new Regex(patterns.Inclusion, RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
			if (patterns.Exclusion != null) {
				this.exclusiveFilter = new Regex(patterns.Exclusion, RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
		}

		public bool IsMatch(string name) {
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
