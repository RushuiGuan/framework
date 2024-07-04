using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class TextExpression : SyntaxNode, IExpression {
		public TextExpression(string text) {
			Text = text;
		}

		public string Text { get; }
		public override IEnumerable<ISyntaxNode> Children => [];

		public override TextWriter Generate(TextWriter writer) {
			writer.AppendLine(Text);
			return writer;
		}
	}
}
