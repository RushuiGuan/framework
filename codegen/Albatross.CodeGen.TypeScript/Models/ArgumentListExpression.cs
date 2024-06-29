using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public record class ArgumentListExpression : Expression {
		internal ArgumentListExpression(SyntaxTree syntaxTree) : base(syntaxTree) { }

		public required IEnumerable<Expression> Arguments { get; init; }
		public override TextWriter Generate(TextWriter writer) {
			writer.WriteItems(Arguments, ", ", (w, item) => w.Code(item));
			return writer;
		}
	}
}
