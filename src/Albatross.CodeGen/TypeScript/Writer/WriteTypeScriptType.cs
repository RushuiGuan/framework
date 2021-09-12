using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteTypeScriptType : CodeGeneratorBase<TypeScriptType> {
		public override void Run(TextWriter writer, TypeScriptType type) {
			if (type.IsVoid && !type.IsAsync) {
				writer.Append("void");
			} else {
				writer.Append(type.Name);
				if (type.IsGeneric) {
					if (type.GenericTypeArguments?.Count() > 0) {
						writer.OpenAngleBracket();
						bool first = true;
						foreach (var genericType in type.GenericTypeArguments) {
							if (!first) {
								writer.Comma().Space();
							} else {
								first = false;
							}
							writer.Run(this, genericType);
						}
						writer.CloseAngleBracket();
					} else {
						throw new CodeGenException("Missing Generic Arguments");
					}
				}
				if (type.IsArray) {
					writer.Append("[]");
				}
			}
		}
	}
}
