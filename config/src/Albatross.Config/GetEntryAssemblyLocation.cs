using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Albatross.Config {
	public class GetEntryAssemblyLocation : IGetEntryAssemblyLocation {
		Assembly assembly;

		public GetEntryAssemblyLocation(Assembly assembly) {
			if (assembly == null) {
				this.assembly = Assembly.GetEntryAssembly();
			} else {
				this.assembly = assembly;
			}
		}

		public string Directory => System.IO.Path.GetDirectoryName(CodeBase);
		public string CodeBase => new Uri(this.assembly.CodeBase).LocalPath;
	}
}
