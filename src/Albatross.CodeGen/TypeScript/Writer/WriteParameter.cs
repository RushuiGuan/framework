using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteParameter : CodeGeneratorBase<Parameter> {
		private readonly ICodeGenerator<TypeScriptType> writeType;
		private readonly ICodeGenerator<AccessModifier> writeAccessModifier;

		public WriteParameter(ICodeGenerator<TypeScriptType> writeType, ICodeGenerator<AccessModifier> writeAccessModifier) {
			this.writeType = writeType;
			this.writeAccessModifier = writeAccessModifier;
		}

		public override void Run(TextWriter writer, Parameter parameter) {
			writer.Run(writeAccessModifier, parameter.AccessModifier).Append(parameter.Name).Space().Run(writeType, parameter.Type);
		}
	}
}
