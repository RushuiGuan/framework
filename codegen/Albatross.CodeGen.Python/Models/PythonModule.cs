using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class PythonModule : ICodeElement {
		public string Name { get; set; }
		public PythonModule(string name) {
			Name = name;
		}

		public List<Class> Classes { get; set; } = new List<Class>();
		public TextWriter Generate(TextWriter writer) {
			foreach(var item in Classes) {
				item.Generate(writer);
			}
			return writer;
		}
	}
}
