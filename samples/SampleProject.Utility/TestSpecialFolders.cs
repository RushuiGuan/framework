using Albatross.Hosting.Utility;
using CommandLine;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("special-folders")]
	public class TestSpecialFoldersOption : MyBaseOption{	}

	public class TestSpecialFolders: UtilityBase<TestSpecialFoldersOption> {
		public TestSpecialFolders(TestSpecialFoldersOption option) : base(option) {		}

		public Task<int> RunUtility() {
			foreach(var value in Enum.GetValues<Environment.SpecialFolder>()) {
				Options.WriteOutput($"{value}: {Environment.GetFolderPath(value)}");
			}
			Options.WriteOutput(Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown");
			return Task.FromResult(0);
		}
	}
}
