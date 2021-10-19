using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CommandLine;
using Albatross.Hosting.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	[Verb("search-reference-tree", HelpText = "Search the reference tree for a particular assembly")]
	public class ReferenceTreeSearchOptions {
		[Option('t', "target-assembly", HelpText = "The target assembly name, partial name will be matched using StartWith", Required = true)]
		public string Target { get; set; }

		[Option('r', "root-assembly", HelpText = "The root assembly where the search will begin", Required = true)]
		public string RootAssembly { get; set; }

		[Option('f', "assembly-folders", HelpText = "Additional assembly folders to search for, delimited by semicolons", Required = false)]
		public string AdditionalAssemblyFolders { get; set; }
	}

	public class ReferenceTreeSearch : UtilityBase<ReferenceTreeSearchOptions> {
		public ReferenceTreeSearch(ReferenceTreeSearchOptions option) : base(option) { }

		public IEnumerable<string> AsssemblyDirectories { get; private set; }

		public override void Init(IConfiguration configuration, IServiceProvider provider) {
			this.AsssemblyDirectories = new string[] {
				new FileInfo(Options.RootAssembly).DirectoryName,
			}.Union((Options.AdditionalAssemblyFolders ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries)).Distinct();
		}

		protected override Task<int> RunUtility() {
			Stack<Assembly> stack = new Stack<Assembly>();
			Assembly assembly = Assembly.LoadFrom(Options.RootAssembly);
			Search(stack, assembly);
			return Task.FromResult(0);
		}

		public bool IsMatch(Assembly assembly) => assembly.GetName().Name.StartsWith(Options.Target, StringComparison.InvariantCultureIgnoreCase);
		public void Print(Stack<Assembly> stack) {
			int count = 0;
			foreach (var item in stack.ToArray().Reverse()) {
				for (int i = 0; i < count; i++) { Console.Write('\t'); }
				var name = item.GetName();
				Console.WriteLine($"{name.Name}, {name.Version}");
				count++;
			}
			Console.WriteLine();
		}
		public bool TryFindAssemblyFile(AssemblyName assemblyReference, out FileInfo file) {
			foreach (var directory in this.AsssemblyDirectories) {
				file = new FileInfo($"{directory}{Path.DirectorySeparatorChar}{assemblyReference.Name}.dll");
				if (file.Exists) {
					return true;
				}
			}
			file = null;
			return false;
		}

		public void Search(Stack<Assembly> stack, Assembly assembly) {
			if (assembly.GetName().Name.StartsWith("System.")) { return; }
			stack.Push(assembly);
			if (IsMatch(assembly)) {
				Print(stack);
			} else {
				foreach (var assemblyReference in assembly.GetReferencedAssemblies()) {
					if (TryFindAssemblyFile(assemblyReference, out FileInfo file)) {
						try {
							Assembly child = Assembly.LoadFile(file.FullName);
							Search(stack, child);
						} catch { }
					}
				}
			}
			stack.Pop();
		}
	}
}
