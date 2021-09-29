using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Writer
{
    public class WriteMethod : CodeGeneratorBase<Method> {
		ICodeGenerator<AccessModifier> writeAccessModifier;
		ICodeGenerator<IEnumerable<Parameter>> writeParams;
		ICodeGenerator<TypeScriptType> writeType;
		ICodeGenerator<CodeBlock> writeCodeBlock;

		public WriteMethod(ICodeGenerator<AccessModifier> writeAccessModifier, ICodeGenerator<IEnumerable<Parameter>> writeParams, 
			ICodeGenerator<TypeScriptType> writeType, ICodeGenerator<CodeBlock> writeCodeBlock) {
			this.writeAccessModifier = writeAccessModifier;
			this.writeParams = writeParams;
			this.writeType = writeType;
			this.writeCodeBlock = writeCodeBlock;
		}

        public override void Run(TextWriter writer, Method method) {
			if (method.AccessModifier != AccessModifier.Public) {
				writer.Run(writeAccessModifier, method.AccessModifier).Space();
			}
            if (method.Async) { writer.Write("async "); }
			writer.Append(method.Name).OpenParenthesis();
			writer.Run(writeParams, method.Parameters);
			writer.CloseParenthesis();
			if (method.ReturnType == TypeScriptType.Void()) {
				writer.Write(":");
				writer.Run(writeType, method.ReturnType).Space();
			}
			writer.Run(writeCodeBlock, method.Body);
			writer.WriteLine();
		}
	}
}