using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.Settings {
	public record class TypeScriptWebClientSettings {
		public SymbolFilterPatterns ControllerFilter { get; init; } = new SymbolFilterPatterns();
		public SymbolFilterPatterns[] ControllerMethodFilters { get; init; } = [];
		public SymbolFilterPatterns DtoFilter { get; init; } = new SymbolFilterPatterns();
		public SymbolFilterPatterns EnumFilter { get; init; } = new SymbolFilterPatterns();

		public string EndPointName { get; init; } = "endpoint_name";
		public string BaseClassName { get; init; } = "WebClient";
		public string BaseClassModule { get; init; } = "@my_module/webclient";
		public string ConfigServiceModule { get; init; } = "@my_module/config";
		public bool UsePromise { get; set; }
		public string EntryFile { get; init; } = "public-api.ts";
		public string SourcePathRelatedToEntryFile { get; init; } = "lib";
		/// <summary>
		/// mapping bewteen c# namespace and typescript module
		/// </summary>
		public Dictionary<string, string> NameSpaceModuleMapping { get; init; } = new Dictionary<string, string>();
		public Dictionary<string, string> TypeMapping { get; init; } = new Dictionary<string, string>();
	}
}