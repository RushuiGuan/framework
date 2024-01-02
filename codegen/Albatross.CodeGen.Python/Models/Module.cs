using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Module : ICodeElement {
		public string Name { get; set; }
		public Module(string name) {
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
