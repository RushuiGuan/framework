using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class BinaryExpression : SyntaxNode, IExpression {
		public override IEnumerable<ISyntaxNode> Children => [this.Left, this.Right,];
		public required IExpression Left { get; init; }
		public required string Operator { get; init; }
		public required IExpression Right { get; init; }

		public override TextWriter Generate(TextWriter writer)
			=> writer.Code(this.Left).Append($" {Operator} ").Code(this.Right);
	}
}