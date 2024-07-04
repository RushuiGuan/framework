using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Syntax {
	public record class ListOfSyntaxNodes<T> : SyntaxNode where T : ISyntaxNode {
		public IEnumerable<T> Nodes { get; init; } = [];
		protected virtual string Separator => ", ";

		public ListOfSyntaxNodes(params T[] nodes) {
			Nodes = nodes;
		}
		public ListOfSyntaxNodes(IEnumerable<T> nodes) {
			Nodes = nodes;
		}

		public override TextWriter Generate(TextWriter writer) {
			writer.WriteItems(Nodes, Separator, (w, item) => w.Code(item));
			return writer;
		}
		public bool HasAny => Nodes.Any();
		public override IEnumerable<ISyntaxNode> Children => Nodes.Cast<ISyntaxNode>();
	}
}