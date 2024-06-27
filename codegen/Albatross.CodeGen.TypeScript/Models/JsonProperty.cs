using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class JsonProperty : ICodeElement {
		public string Name { get; }
		public ICodeElement Expression { get; }

		public JsonProperty(string name, ICodeElement expression) {
			Name = name;
			Expression = expression;
		}


		public TextWriter Generate(TextWriter writer) {
			if (Expression is IdentifierName identifier && identifier.Name == this.Name) {
				writer.Append(this.Name);
			} else {
				writer.Append(this.Name).Append(": ").Code(Expression);
			}
			return writer;
		}
	}
}
