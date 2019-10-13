using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Writer
{
    public class WriteMethod : CodeGeneratorBase<Method> {
		ICodeGenerator<AccessModifier> writeAccessModifier;
		ICodeGenerator<IEnumerable<Parameter>> writeParams;
		ICodeGenerator<DotNetType> writeType;
		ICodeGenerator<CodeBlock> writeCodeBlock;

		public WriteMethod(ICodeGenerator<AccessModifier> writeAccessModifier, ICodeGenerator<IEnumerable<Parameter>> writeParams, ICodeGenerator<DotNetType> writeType, ICodeGenerator<CodeBlock> writeCodeBlock) {
			this.writeAccessModifier = writeAccessModifier;
			this.writeParams = writeParams;
			this.writeType = writeType;
			this.writeCodeBlock = writeCodeBlock;
		}

        public override void Run(TextWriter writer, Method t) {
			writer.Run(writeAccessModifier, t.AccessModifier).Space();
			if (t.Static) {
				writer.Static();
			} else if (t.Override) {
				writer.Write("override ");
			} else if (t.Virtual) {
				writer.Write("virtual ");
			}
            if (t.Async) { writer.Write("async "); }

			writer.Run(writeType, t.ReturnType).Space();

			writer.Append(t.Name).OpenParenthesis();
			writer.Run(writeParams, t.Parameters);
			writer.CloseParenthesis();
			writer.Run(writeCodeBlock, t.Body);
		}
	}
}