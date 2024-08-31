using System.CommandLine;

namespace Albatross.CommandLine {
	public static class Extensions {
		public static Option<T> WithAlias<T>(this Option<T> option, params string[] aliases) {
			foreach(var alias in aliases) {
				option.AddAlias(alias);
			}
			return option;
		}
	}
}
