using Albatross.CodeGen.Core;
using Albatross.Reflection;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Interface : ICodeElement {
		public Interface(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
		public bool IsGeneric { get; set; }
		public List<string> GenericTypes { get; set; } = new List<string>();
		public List<Property> Properties { get; set; } = new List<Property>();

		public TextWriter Generate(TextWriter writer) {
			writer.Append("export ").Append("interface ");
			if (IsGeneric) {
				writer.Append(Name.GetGenericTypeName());
				writer.Append("<");
				writer.WriteItems(GenericTypes, ",");
				writer.Append(">");
			} else {
				writer.Append(Name);
			}
			using (var scope = writer.BeginScope()) {
				foreach (var property in Properties) {
					scope.Writer.Code(property);
				}
			}
			writer.WriteLine();
			return writer;
		}
	}
}
