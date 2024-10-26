using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Models {
	public class ForEachCodeBlock : ICodeElement {
		public ForEachCodeBlock(string itemVariable, string collectionVariable) {
			ItemVariable = itemVariable;
			CollectionVariable = collectionVariable;
		}
		public string ItemVariable { get; set; }
		public string CollectionVariable { get; set; }
		public ICodeElement ForEachContent { get; set; } = new CodeBlock();

		public TextWriter Generate(TextWriter writer) {
			using (var scope = writer.Append("foreach (")
				.Append("var ").Append(ItemVariable).Append(" in ").Append(CollectionVariable).Append(")")
				.BeginScope()) {
				scope.Writer.Code(ForEachContent);
			}
			writer.WriteLine();
			return writer;
		}
	}
}