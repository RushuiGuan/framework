using Albatross.Text;
using System;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class IfCodeBlock : ICodeElement  {
		public string Condition { get; set; }
		public ICodeElement CodeBlock { get; set; } = new CodeBlock();

		public IfCodeBlock(string condition) {
			this.Condition = condition;
		}
		public IfCodeBlock(string condition, ICodeElement codeBlock) {
			this.Condition = condition;
			this.CodeBlock = codeBlock;
		}

		public TextWriter Generate(TextWriter writer) {
			var scope = writer.Append("if (").Append(Condition).Append(")").BeginScope();
			scope.Writer.Code(CodeBlock);
			return writer;
		}
	}
}
