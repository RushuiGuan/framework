using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteCodeBlock : CodeGeneratorBase<CodeBlock> {
		public override void Run(TextWriter writer, CodeBlock source) {
			using (var scope = writer.BeginScope()) {
				if (!string.IsNullOrEmpty(source.Content)) {
					scope.Writer.WriteLine(source.Content);
				}
				foreach (var child in source.Children) {
					Run(scope.Writer, child);
				}
			}
		}
	}
	public class WriteIfCodeBlock : CodeGeneratorBase<IfCodeBlock> {
		public override void Run(TextWriter writer, IfCodeBlock source) {
			var scope = writer.Append("if (").Append(source.Expression).Append(")").BeginScope();
			using (scope) { 
				if (!string.IsNullOrEmpty(source.Content)) {
					scope.Writer.WriteLine(source.Content);
				}
				foreach (var child in source.Children) {
					Run(scope.Writer, child);
				}
			}
		}
	}
}