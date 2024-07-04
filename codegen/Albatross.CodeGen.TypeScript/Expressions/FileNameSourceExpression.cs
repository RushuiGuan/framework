using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	// should validate the name using regex
	public record class FileNameSourceExpression : SyntaxNode, ISourceExpression {
		public FileNameSourceExpression(string name) {
			this.FileName = name;
		}
		public string FileName { get; }

		public override IEnumerable<ISyntaxNode> Children => [];

		public override TextWriter Generate(TextWriter writer) {
			writer.Append('\'').Append(FileName).Append('\'');
			return writer;
		}
	}
}
