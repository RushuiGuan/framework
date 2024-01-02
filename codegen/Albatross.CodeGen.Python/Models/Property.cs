using System;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Property : ICodeElement {
		public string Name { get; set; }
		public PythonType? Type { get; set; }
		public ICodeElement Value { get; }

		public Property(string name, PythonType? type, ICodeElement value) {
			this.Name = name;
			this.Type = type;
			Value = value;
		}
		public TextWriter Generate(TextWriter writer) {
			return writer;
		}
	}
}
