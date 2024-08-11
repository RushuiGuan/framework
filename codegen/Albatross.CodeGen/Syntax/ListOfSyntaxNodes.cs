using Albatross.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Syntax {
	public record class ListOfSyntaxNodes<T> : SyntaxNode, IEnumerable<T> where T : ISyntaxNode {
		public T[] Nodes { get; init; } = [];
		protected virtual string Separator => ", ";
		public string Padding { get; init; } = string.Empty;
		public bool Unique { get; init; }
		public bool Sorted { get; init; }

		public ListOfSyntaxNodes(params T[] nodes) {
			Nodes = nodes;
		}
		public ListOfSyntaxNodes(IEnumerable<T> nodes) {
			Nodes = nodes.ToArray();
		}
		public int Count => Nodes.Length;

		public override TextWriter Generate(TextWriter writer) {
			var nodes = this.Nodes.AsEnumerable();
			if (Unique) {
				nodes = nodes.Distinct();
			}
			if (Sorted) {
				nodes = nodes.OrderBy(x=>x.ToString());
			}
			writer.WriteItems(nodes, Separator, (w, item) => w.Code(item), this.Padding, this.Padding);
			return writer;
		}

		public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Nodes).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Nodes.GetEnumerator();
		public bool HasAny => Nodes.Any();
		public override IEnumerable<ISyntaxNode> Children => Nodes.Cast<ISyntaxNode>();
	}
}