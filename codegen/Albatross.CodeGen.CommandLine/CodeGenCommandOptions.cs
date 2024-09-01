using Albatross.CommandLine;
using System.IO;

namespace Albatross.CodeGen.CommandLine {
	[Verb("typescript-dto", typeof(TypeScriptDtoCodeGenHandler))]
	[Verb("typescript-proxy", typeof(TypeScriptProxyCodeGenHandler))]
	// [Verb("csharp-proxy")]
	public class CodeGenCommandOptions {
		[Option(Alias = ["p"])]
		public FileInfo ProjectFile { get; set; } = null!;

		[Option(Alias = ["s"])]
		public FileInfo? SettingsFile { get; set; }

		[Option(Alias = ["o"])]
		public DirectoryInfo? OutputDirectory { get; set; }
	}
}
