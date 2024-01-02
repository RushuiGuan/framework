using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Class : ICodeElement {
		public string Name { get; set; }
		public Class[] BaseClass { get; set; } = Array.Empty<Class>();

		public Class(string name) {
			Name = name;
		}

		public List<Method> Methods { get; set; } = new List<Method>();
		public List<Property> Properties { get; set; } = new List<Property>();
		public List<Field> Fields { get; set; } = new List<Field>();

		public TextWriter Generate(TextWriter writer) {
			writer.Append("class ").Append(Name);
			if (BaseClass.Any()) {
				writer.OpenParenthesis().WriteItems(BaseClass.Select(x=>x.Name), ", ").CloseParenthesis();
			}
			using(var scope = writer.BeginPythonScope()) {
				foreach(var item in Methods) {
					item.Generate(scope.Writer);
				}
			}
			return writer;
		}
	}
}
