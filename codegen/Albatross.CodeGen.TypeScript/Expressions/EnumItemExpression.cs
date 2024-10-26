using Albatross.CodeGen.Syntax;
using Albatross.Collections;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class EnumItemExpression : SyntaxNode, IExpression {
		public EnumItemExpression(string name) {
			this.Identifier = new IdentifierNameExpression(name);
		}
		public IdentifierNameExpression Identifier { get; }
		public SyntaxNode? Expression { get; init; }

		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> { Identifier, }.AddIfNotNull(Expression);


		public override TextWriter Generate(TextWriter writer) {
			if (Expression == null) {
				writer.Code(Identifier);
			} else {
				writer.Code(Identifier).Append(" = ").Code(Expression);
			}
			return writer;
		}
	}
}