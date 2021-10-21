using Albatross.CodeGen.Core;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Class : ICodeElement {
		public Class(string name) {
			this.Name = name;
		}
		public string Name { get; set; }

		public AccessModifier AccessModifier { get; set; }
		public Class? BaseClass { get; set; }
		public bool IsGeneric { get; set; }

		public Constructor? Constructor { get; set; }
		public IEnumerable<Import> Imports { get; set; } = new Import[0];
		public IEnumerable<Property> Properties { get; set; } = new Property[0];
		public IEnumerable<Method> Methods { get; set; } = new Method[0];

		public TextWriter Generate(TextWriter writer) {
			foreach (var import in Imports) {
				writer.Code(import);
			}

			writer.Append("export ").Append("class ");
			using (var scope = writer.BeginScope(Name)) {
				foreach (var property in Properties) {
					scope.Writer.Code(property);
				}
				if (Constructor != null) {
					scope.Writer.Code(Constructor);
				}
				foreach (var method in Methods) {
					scope.Writer.Code(method);
				}
			}
			writer.WriteLine();
			return writer;
		}
	}
}
