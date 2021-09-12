using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteAccessModifier: CodeGeneratorBase<AccessModifier> {
		public override void Run(TextWriter writer, AccessModifier accessModifier) {
			switch (accessModifier) {
				case AccessModifier.Public:
					writer.Write(" public ");
					break;
				case AccessModifier.Private:
					writer.Write(" private ");
					break;
				case AccessModifier.Protected:
					writer.Write(" protected ");
					break;
			}
		}
	}
}
