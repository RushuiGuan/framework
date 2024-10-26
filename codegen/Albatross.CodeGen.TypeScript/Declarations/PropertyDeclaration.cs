using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class PropertyDeclaration : SyntaxNode, IDeclaration, ICodeElement {
		public PropertyDeclaration(string name) {
			this.Identifier = new IdentifierNameExpression(name);
		}

		public IdentifierNameExpression Identifier { get; }
		public bool Optional { get; init; }
		public IEnumerable<IModifier> Modifiers { get; init; } = [];
		public ITypeExpression Type { get; init; } = Defined.Types.Any();
		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> { Identifier, Type };

		public override TextWriter Generate(TextWriter writer) {
			writer.Code(Identifier);
			if (Optional) {
				writer.Append(" ?");
			};
			writer.Append(": ").Code(Type);
			return writer;
		}
	}
}