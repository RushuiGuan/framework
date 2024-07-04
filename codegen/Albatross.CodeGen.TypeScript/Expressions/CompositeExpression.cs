using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class CompositeExpression : SyntaxNode, IExpression {
		public CompositeExpression(params IExpression[] items) {
			this.Items = items;
		}
		public CompositeExpression(IEnumerable<IExpression> items) {
			Items = items;
		}
		public IEnumerable<IExpression> Items { get; init; }
		public override TextWriter Generate(TextWriter writer) {
			foreach (var item in Items) {
				writer.Code(item).AppendLine();
			}
			return writer;
		}
		public override IEnumerable<ISyntaxNode> Children => Items.Cast<ISyntaxNode>();
	}
}