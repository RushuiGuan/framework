using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.Settings {
	public record class CSharpWebClientSettings {
		public SymbolFilterPatterns ControllerFilter { get; init; } = new SymbolFilterPatterns();

		public string Namespace { get; init; } = "MyNamespace";
		/// <summary>
		/// If true, the client will use Text/Plain content type for string post.  This requires the server to accept 
		/// text/plain content type for the Body.  If false, the client will use application/json content type for string post.
		/// </summary>
		public bool UseTextContentTypeForStringPost { get; init; } = true;
	}
}