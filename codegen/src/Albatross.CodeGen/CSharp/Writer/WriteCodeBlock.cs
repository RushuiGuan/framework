using System;
using System.IO;
using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;

namespace Albatross.CodeGen.CSharp.Writer {
	public class WriteCodeBlock : CodeGeneratorBase<CodeBlock> {
		public override void Run(TextWriter writer, CodeBlock source) {
			using (var scope = new CodeGeneratorScope(writer, args => args.WriteLine(" {"), args => args.Write("}"))) {
				if (!string.IsNullOrEmpty(source?.Content)) {
					scope.Writer.WriteLine(source.Content);
				}
			}
		}
	}
}
