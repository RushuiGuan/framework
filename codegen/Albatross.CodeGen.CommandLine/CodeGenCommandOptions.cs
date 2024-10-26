using Albatross.CommandLine;
using System.IO;

namespace Albatross.CodeGen.CommandLine {
	[Verb("csharp-proxy", typeof(CSharpWebClientCodeGenCommandHandler_Client740), Description = "Generate CSharp Http Proxy class that works with the current version of Albatross.WebClient assembly")]
	[Verb("csharp-proxy-client402", typeof(CSharpWebClientCodeGenCommandHandler_Client402), Description = "Generate CSharp Http Proxy class that works with Albatross.WebClient assembly version 4.0.*")]
	[Verb("typescript-dto", typeof(TypeScriptDtoCodeGenCommandHandler))]
	[Verb("typescript-proxy", typeof(TypeScriptWebClientCodeGenCommandHandler))]
	[Verb("controller-model", typeof(ControllerInfoModelGenerator))]
	[Verb("dto-model", typeof(DtoClassInfoModelGenerator))]
	public record class CodeGenCommandOptions {
		[Option("p")]
		public FileInfo ProjectFile { get; set; } = null!;

		[Option("s")]
		public FileInfo? SettingsFile { get; set; }

		[Option("o")]
		public DirectoryInfo? OutputDirectory { get; set; }

		[Option("c")]
		public string? AdhocFilter { get; set; }
	}
}