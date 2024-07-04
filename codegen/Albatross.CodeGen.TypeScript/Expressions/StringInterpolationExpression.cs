using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class StringInterpolationExpression : SyntaxNode, IExpression {
		public StringInterpolationExpression(string text, params IdentifierNameExpression[] identifiers) {
			this.Identifiers = identifiers;
			var sb = new StringBuilder(text);
			for (int i = 0; i < Identifiers.Length; i++) {
				sb.Replace("${{{i}}}", Identifiers[i].Name);
			}
			this.Text = sb.ToString();
		}
		public StringInterpolationExpression(string text) {
			this.Text = text;
		}
		public IdentifierNameExpression[] Identifiers { get; } = [];
		public string Text { get; }

		public override IEnumerable<ISyntaxNode> Children => Identifiers;

		public override TextWriter Generate(TextWriter writer) {
			writer.Append("`").Append(this.Text).Append("`");
			return writer;
		}
	}
}
