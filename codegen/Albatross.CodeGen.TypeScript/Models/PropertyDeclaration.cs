using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class PropertyDeclaration : ICodeElement{
		public PropertyDeclaration(string name, TypeExpression type) {
			this.Name = name;
			this.Type = type;
		}
		public bool IsPrivate { get; set; }
		public string Name { get; set; }
		public TypeExpression Type { get; set; }
		public bool Optional { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			if (Optional) { 
				writer.Append("?"); 
			}
			writer.Append(": ").Code(Type).Append(";").WriteLine();
			return writer;
		}
	}
}
