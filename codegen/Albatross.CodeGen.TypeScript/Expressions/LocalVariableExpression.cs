using Albatross.Collections;
using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class LocalVariableExpression : SyntaxNode, IExpression, ICodeElement {
		public LocalVariableExpression(string name) {
			Identifier = new IdentifierNameExpression(name);
		}

		public IdentifierNameExpression Identifier { get; }
		public ITypeExpression Type { get; init; } = Defined.Types.Any;
		public IEnumerable<IModifier> Modifiers { get; init; } = [];
		public SyntaxNode? Assignment { get; init; }

		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> { Identifier, Type }.AddIfNotNull(Assignment);

		public override TextWriter Generate(TextWriter writer) {
			writer.Append("var ").Code(Identifier).Append(" : ").Code(Type);
			if (Assignment != null) {
				writer.Append(" = ").Code(Assignment);
			}
			writer.AppendLine(";");
			return writer;
		}
	}
}
