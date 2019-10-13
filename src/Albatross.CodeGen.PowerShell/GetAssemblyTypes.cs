using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.PowerShell {
	public class GetAssemblyTypes {
		IEnumerable<string> additionalPaths;

		public GetAssemblyTypes(IEnumerable<string> additionalPaths) {
			this.additionalPaths = additionalPaths;
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler((obj, asmArgs) => LoadFromPaths(asmArgs));
		}

		Assembly LoadFromPaths(ResolveEventArgs args) {
			foreach (string item in additionalPaths) {
				string name = Path.Combine(new DirectoryInfo(item).FullName, new AssemblyName(args.Name).Name + ".dll");
				if (File.Exists(name)) {
					return Assembly.LoadFrom(name);
				}
			}
			return null;
		}
		public Type LoadTypeByName(string typeName) {
			return Type.GetType(typeName, true);
		}

		public void LoadAssemblyTypes(string targetAssembly, Action<Type> action) {
			if (!File.Exists(targetAssembly)) { throw new Exception($"codegen: Assembly file {targetAssembly} doesn't exist"); }
			var assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(targetAssembly);
			foreach (Type type in assembly.GetTypes()) {
				action(type);
			}
		}
	}
}