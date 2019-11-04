using CommandLine;
using System;
using System.IO;
using System.Reflection;

namespace Albatross.Host.Console {
	[Verb("reference-tree", HelpText = "Print out the reference tree of the inputted assembly file")]
	public class GetReferenceTreeOption {
		[Option('f', "assembly-file", Required = true, HelpText = "Location of the target assembly")]
		public string AssemblyFile { get; set; }
	}

	class Program {
		static int Main(string[] args) {
			return Parser.Default
				.ParseArguments<GetReferenceTreeOption>(args)
				.MapResult<GetReferenceTreeOption, int>(
					opt => new GetReferenceTree().Init(opt).Run(),
					err => 1);
		}
	}
	public class GetReferenceTree {
		GetReferenceTreeOption option;
		string location;
		public GetReferenceTree Init(GetReferenceTreeOption option) {
			this.option = option;
			return this;
		}

		public int Run() {
			location = Path.GetDirectoryName(option.AssemblyFile);
			Environment.CurrentDirectory = location;
			Assembly asm = Assembly.LoadFile(option.AssemblyFile);
			PrintTree(asm, 0);
			return 0;
		}

		public void PrintTree(Assembly assembly, int indent) {
			for(int i=0; i<indent; i++) {
				System.Console.Write('\t');
			}
			System.Console.WriteLine($"{assembly.GetName().Name}, {assembly.GetName().Version}");
			foreach (var reference in assembly.GetReferencedAssemblies()) {
				if (reference.Name.StartsWith("System.")) {
					continue;
				}
				string file = $"{location}{Path.DirectorySeparatorChar}{reference.Name}.dll";
				Assembly child=null;
				if (File.Exists(file)) {
					try {
						child = Assembly.LoadFile(file);
					} catch { }
				} else {
					child = Assembly.Load(reference);
				}
				if (child != null) {
					PrintTree(child, indent + 1);
				}
			}
		}
	}
}
