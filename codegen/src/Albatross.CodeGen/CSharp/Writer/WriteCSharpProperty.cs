using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.CSharp.Writer
{
	public class WriteCSharpProperty : CodeGeneratorBase<Property> {
		ICodeGenerator<AccessModifier> writeAccessModifier;
		ICodeGenerator<DotNetType> writeType;
		WriteCodeBlock writeCodeBlock;

		public WriteCSharpProperty(ICodeGenerator<AccessModifier> renderAccessModifier, ICodeGenerator<DotNetType> renderType, WriteCodeBlock writeCodeBlock) {
			this.writeAccessModifier = renderAccessModifier;
			this.writeType = renderType;
			this.writeCodeBlock = writeCodeBlock;
		}

		public override void Run(TextWriter writer, Property property) {
            writer.Run(writeAccessModifier, property.Modifier).Space();
			if (property.Static) { writer.Static(); }

            writer.Run(writeType, property.Type).Space().Append(property.Name);

            using (var scope = writer.BeginScope()) {
				if (property.CanRead) {
					scope.Writer.Append("get");
					if (property.GetCodeBlock != null) {
						scope.Writer.Run(writeCodeBlock, property.GetCodeBlock);
					} else {
						scope.Writer.Write(";");
					}
				}
				if (property.CanWrite) {
					if (property.SetModifier != property.Modifier) {
						scope.Writer.Run(writeAccessModifier, property.SetModifier).Space();
					}
					scope.Writer.Append("set");
					if (property.SetCodeBlock != null) {
						scope.Writer.Run(writeCodeBlock, property.SetCodeBlock);
					} else {
						scope.Writer.Write(";");
					}
				}
			}
		}
	}
}
