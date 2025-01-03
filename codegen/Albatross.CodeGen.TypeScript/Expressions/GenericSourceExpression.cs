using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class GenericSourceExpression : SyntaxNode, ISourceExpression {
		public GenericSourceExpression(string name) {
			Name = name;
		}
		public override IEnumerable<ISyntaxNode> Children => [];

		public string Name { get; }
		public override TextWriter Generate(TextWriter writer) {
			writer.Append('"').Append(Name).Append('"');
			return writer;
		}
		public override string ToString() => Name;
	}
}