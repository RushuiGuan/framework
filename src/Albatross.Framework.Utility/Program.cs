using CommandLine;

namespace Albatross.Framework.Utility {
	class Program {
		static int Main(string[] args) {
			return Parser.Default
				.ParseArguments<ReferenceTreeSearchOptions>(args)
				.MapResult<ReferenceTreeSearchOptions, int>(
					opt => new ReferenceTreeSearch().Init(opt).Run(),
					err => 1);
		}
	}
}
