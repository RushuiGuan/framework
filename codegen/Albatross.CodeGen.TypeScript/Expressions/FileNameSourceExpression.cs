using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System;
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
			if (Path.IsPathRooted(FileName)) {
				throw new ArgumentException("FileNameSource can only use relative path for its FileName property");
			}
			writer.Append('\'').Append("./");
			var directory = Path.GetDirectoryName(FileName)?.Replace('\\', '/') ?? string.Empty;
			if (directory.StartsWith("./")) {
				directory = directory.Substring(2);
			}
			if (!string.IsNullOrEmpty(directory) && directory != ".") {
				writer.Append(directory).Append('/');
			}
			writer.Append(Path.GetFileNameWithoutExtension(FileName)).Append('\'');
			return writer;
		}
		public override string ToString() => this.FileName;
	}
}