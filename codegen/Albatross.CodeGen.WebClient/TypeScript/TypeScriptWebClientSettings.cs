using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public record class TypeScriptWebClientSettings {
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