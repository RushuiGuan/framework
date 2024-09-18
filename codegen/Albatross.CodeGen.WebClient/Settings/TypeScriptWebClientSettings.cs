using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.Settings {
	public record class TypeScriptWebClientSettings {
		public SymbolFilterPatterns ControllerFilter { get; init; } = new SymbolFilterPatterns();
		public SymbolFilterPatterns DtoFilter { get; init; } = new SymbolFilterPatterns();
		public SymbolFilterPatterns EnumFilter { get; init; } = new SymbolFilterPatterns();

		public string EndPointName { get; init; } = "endpoint_name";
		public string BaseClassName { get; init; } = "WebClient";
		public string BaseClassModule { get; init; } = "@my_module/webclient";
		public string ConfigServiceModule { get; init; } = "@my_module/config";
		public bool UsePromise { get; set; }
		/// <summary>
		/// mapping bewteen c# namespace and typescript module
		/// </summary>
		public Dictionary<string, string> NameSpaceModuleMapping { get; init; } = new Dictionary<string, string>();
		public string? ImplementationAssembly { get; init; }
	}
}