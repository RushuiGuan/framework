using Albatross.CommandLine;

namespace Albatross.CodeGen.CommandLine {
	[Verb("typescript-dto", typeof(TypeScriptDtoCodeGenHandler))]
	[Verb("typescript-proxy", typeof(TypeScriptProxyCodeGenHandler))]
	// [Verb("csharp-proxy")]
	public class CodeGenCommandOptions {
		[Option(Alias =["p"])]
		public string ProjectFile { get; set; } = string.Empty;

		[Option(Alias = ["s"])]
		public string? SettingsFile { get; set; }

		[Option(Alias = ["o"])]
		public string? OutputDirectory { get; set; }
	}
}
