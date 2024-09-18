using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.Settings {
	public record class CSharpWebClientSettings {
		public SymbolFilterPatterns ControllerFilter { get; init; } = new SymbolFilterPatterns();

		public string Namespace { get; init; } = "MyNamespace";
	}
}