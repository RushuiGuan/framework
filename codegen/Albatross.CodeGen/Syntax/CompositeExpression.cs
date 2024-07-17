using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Syntax {
	public record class CompositeExpression : SyntaxNode, IExpression {
		public CompositeExpression(params IExpression[] items) {
			this.Items = items;
		}
		public CompositeExpression(IEnumerable<IExpression> items) {
			Items = items;
		}
		public IEnumerable<IExpression> Items { get; init; }
		public override TextWriter Generate(TextWriter writer) {
			writer.WriteItems(Items, "\n", (w, x) => w.Code(x));
			return writer;
		}
		public override IEnumerable<ISyntaxNode> Children => Items.Cast<ISyntaxNode>();
	}
}