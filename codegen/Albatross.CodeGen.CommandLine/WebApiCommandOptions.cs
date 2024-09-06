using Albatross.CommandLine;
using System.IO;

namespace Albatross.CodeGen.CommandLine {
	[Verb("webapi", typeof(WebApiCodeGenHandler))]
	public record class WebApiCommandOptions {
		[Option(Alias = ["p"])]
		public FileInfo ProjectFile { get; set; } = null!;

		[Option(Alias = ["c"])]
		public string? Controller { get; set; }
	}
}
