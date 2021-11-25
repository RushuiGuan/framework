using Albatross.CodeGen.Core;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Enum : ICodeElement {
		public Enum(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
		public IEnumerable<string> Values { get; set; } = new string[0];

		public TextWriter Generate(TextWriter writer) {
			writer.Append("export ");
			writer.Append("enum ");
			using (var scope = writer.BeginScope(Name)) {
				foreach (var value in Values) {
					scope.Writer.Append(value).WriteLine(",");
				}
			}
			writer.WriteLine();
			return writer;
		}
	}
}
