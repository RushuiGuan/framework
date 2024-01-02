using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Class : ICodeElement {
		public string Name { get; set; }
		public Class(string name) {
			Name = name;
		}

		public List<Method> Methods { get; set; } = new List<Method>();
		public TextWriter Generate(TextWriter writer) {
			writer.Append("class ").Append(Name);
			using(var scope = writer.BeginPythonScope()) {
				foreach(var item in Methods) {
					item.Generate(scope.Writer);
				}
			}
			return writer;
		}
	}
}
