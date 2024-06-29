using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class EnumDeclaration : ICodeElement {
		public EnumDeclaration(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
		public IEnumerable<string> Values { get; set; } = new string[0];
		public bool UseTextValue { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append("export ");
			writer.Append("enum ");
			using (var scope = writer.BeginScope(Name)) {
				foreach (var value in Values) {
					scope.Writer.Append(value);
					if (UseTextValue) {
						scope.Writer.Append(" = ").Literal(value);
					}
					scope.Writer.WriteLine(",");
				}
			}
			writer.WriteLine();
			return writer;
		}
	}
}
