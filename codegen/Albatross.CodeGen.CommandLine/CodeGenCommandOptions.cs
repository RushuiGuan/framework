using Albatross.CommandLine;
using System.IO;

namespace Albatross.CodeGen.CommandLine {
	[Verb("csharp-proxy-webclient740", typeof(CSharpWebClientCodeGenCommandHandler_WebClient740), Description ="Generate CSharp Http Proxy class.  The resulting proxy will work with Albatross.WebClient assembly version 7.4.*")]
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
