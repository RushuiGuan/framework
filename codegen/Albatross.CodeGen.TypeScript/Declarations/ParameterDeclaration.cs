using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.Modifiers;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class ParameterDeclaration : SyntaxNode, IDeclaration, ICodeElement {
		public ParameterDeclaration(string name) {
			this.Identifier = new IdentifierNameExpression(name);
		}
		public bool Optional { get; init; } = false;
		public required ITypeExpression Type { get; init; }
		public IEnumerable<IModifier> Modifiers { get; init; } = [];
		public IdentifierNameExpression Identifier { get; }

		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> { Type, Identifier };

		public override TextWriter Generate(TextWriter writer) {
			var items = Modifiers.Where(x => x is AccessModifier).ToArray();
			if (items.Length > 1) {
				throw new InvalidOperationException("AccessModifier can only be specified once");
			} else if (items.Length == 1) {
				writer.Append(items[0].Name).Space();
			}
			writer.Code(Identifier).Append(": ").Code(Type);
			return writer;
		}
	}
}