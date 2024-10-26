using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.WebClient.Settings {
	public static class Extensions {
		public static Dictionary<string, SymbolFilterPatterns> Overwrite(this Dictionary<string, SymbolFilterPatterns> changes,
			Dictionary<string, SymbolFilterPatterns> destination) {

			var result = new Dictionary<string, SymbolFilterPatterns>(destination);
			foreach (var pair in changes) {
				if (pair.Value.HasValue) {
					result[pair.Key] = pair.Value;
				}
			}
			return result;
		}

		public static SymbolFilter[] CreateCSharpControllerMethodFilters(this CodeGenSettings settings) {
			var selected = settings.CSharpWebClientSettings.ControllerMethodFilters.Where(x => x.HasValue).ToArray();
			if (selected.Length == 0) {
				selected = settings.ControllerMethodFilters.Where(x => x.HasValue).ToArray();
			}
			return selected.Select(x => new SymbolFilter(x)).ToArray();
		}
		public static SymbolFilter[] CreateTypeScriptControllerMethodFilters(this CodeGenSettings settings) {
			var selected = settings.TypeScriptWebClientSettings.ControllerMethodFilters.Where(x => x.HasValue).ToArray();
			if (selected.Length == 0) {
				selected = settings.ControllerMethodFilters.Where(x => x.HasValue).ToArray();
			}
			return selected.Select(x => new SymbolFilter(x)).ToArray();
		}
		public static SymbolFilter[] CreateControllerMethodFilters(this CodeGenSettings settings)
			=> settings.ControllerMethodFilters.Where(x => x.HasValue).Select(x => new SymbolFilter(x)).ToArray();

		public static SymbolFilterPatterns CreateCSharpControllerFilter(this CodeGenSettings settings)
			=> settings.CSharpWebClientSettings.ControllerFilter.Overwrite(settings.ControllerFilter);

		public static SymbolFilterPatterns CreateTypeScriptControllerFilter(this CodeGenSettings settings)
			=> settings.TypeScriptWebClientSettings.ControllerFilter.Overwrite(settings.ControllerFilter);

		public static SymbolFilterPatterns CreateTypeScriptDtoFilter(this CodeGenSettings settings)
			=> settings.TypeScriptWebClientSettings.DtoFilter.Overwrite(settings.DtoFilter);

		public static SymbolFilterPatterns TypeScriptEnumFilter(this CodeGenSettings settings)
			=> settings.TypeScriptWebClientSettings.EnumFilter.Overwrite(settings.EnumFilter);
	}
}