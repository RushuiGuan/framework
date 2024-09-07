using Albatross.CommandLine;
using System.IO;

namespace Albatross.CodeGen.CommandLine {
	[Verb("csharp-proxy", typeof(CSharpProxyCodeGenCommandHandler))]
	[Verb("typescript-dto", typeof(TypeScriptDtoCodeGenCommandHandler))]
	[Verb("typescript-proxy", typeof(TypeScriptProxyCodeGenCommandHandler))]
	public record class CodeGenCommandOptions {
		[Option(Alias = ["p"])]
		public FileInfo ProjectFile { get; set; } = null!;

		[Option(Alias = ["s"])]
		public FileInfo? SettingsFile { get; set; }

		[Option(Alias = ["o"])]
		public DirectoryInfo? OutputDirectory { get; set; }

		[Option(Alias = ["c"])]
		public string? Controller { get; set; }
	}
}
