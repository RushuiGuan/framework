using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.CSharp.Writer
{
	public class WriteConstructor : CodeGeneratorBase<Constructor> {
		ICodeGenerator<AccessModifier> writeAccessModifier;
		ICodeGenerator<IEnumerable<Parameter>> writeParams;
		ICodeGenerator<CodeBlock> writeCodeBlock;

		public WriteConstructor(ICodeGenerator<AccessModifier> writeAccessModifier, ICodeGenerator<IEnumerable<Parameter>> writeParams, ICodeGenerator<CodeBlock> writeCodeBlock) {
			this.writeAccessModifier = writeAccessModifier;
			this.writeParams = writeParams;
			this.writeCodeBlock = writeCodeBlock;
		}


		public override void Run(TextWriter writer, Constructor constructor) {
			if (constructor.Static) {
				writer.Static();
			} else {
				writer.Run(writeAccessModifier, constructor.AccessModifier).Space();
			}
			writer.Append(constructor.Name).OpenParenthesis();
			writer.Run(writeParams, constructor.Parameters);
			writer.CloseParenthesis();

			if(constructor.BaseConstructor!=null) {
				writer.Append(" : ").Append(constructor.BaseConstructor.Name).OpenParenthesis();
                writer.WriteItems(constructor.Parameters, ", ", (w, item)=> { w.Append("@").Append(item.Name); });
				writer.CloseParenthesis();
			}
			writer.Run(writeCodeBlock, constructor.Body);
		}
	}
}