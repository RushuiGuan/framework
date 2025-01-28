using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.Settings {
	public record class CodeGenSettings {
		public TypeScriptWebClientSettings TypeScriptWebClientSettings { get; init; } = new TypeScriptWebClientSettings();
		public CSharpWebClientSettings CSharpWebClientSettings { get; init; } = new CSharpWebClientSettings();

		public SymbolFilterPatterns ControllerFilter { get; init; } = new SymbolFilterPatterns();
		public SymbolFilterPatterns DtoFilter { get; init; } = new SymbolFilterPatterns();
		public SymbolFilterPatterns EnumFilter { get; init; } = new SymbolFilterPatterns();
		public SymbolFilterPatterns[] ControllerMethodFilters { get; init; } = [];
	}
}