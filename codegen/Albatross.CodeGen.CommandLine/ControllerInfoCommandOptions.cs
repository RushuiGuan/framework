using Albatross.CommandLine;
using System.IO;

namespace Albatross.CodeGen.CommandLine {
	[Verb("controller", typeof(ControllerInfoConverterCommandHandler))]
	public record class ControllerInfoCommandOptions {
		[Option(Alias = ["p"])]
		public FileInfo ProjectFile { get; set; } = null!;

		[Option(Alias = ["c"])]
		public string? Controller { get; set; }
	}
}
