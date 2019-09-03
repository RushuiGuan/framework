using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.CSharp.Writer
{
	public class WriteField : CodeGeneratorBase<Field> {
		ICodeGenerator<AccessModifier> writeAccessModifier;
		ICodeGenerator<DotNetType> writeType;

		public WriteField(ICodeGenerator<AccessModifier> renderAccessModifier, ICodeGenerator<DotNetType> renderType) {
			this.writeAccessModifier = renderAccessModifier;
			this.writeType = renderType;
		}

		public override void Run(TextWriter writer, Field field) {
			if (field.Modifier != AccessModifier.None) {
				writer.Run(writeAccessModifier, field.Modifier).Space();
			}
			if (field.Const) { writer.Const(); }
			if (field.ReadOnly) { writer.ReadOnly(); }
			if (field.Static) { writer.Static(); }
            writer.Run(writeType, field.Type).Space().Append(field.Name);
			if (!string.IsNullOrEmpty(field.Value)) {
				writer.Append(" = ").Append(field.Value);
			}
			writer.Semicolon();
		}
	}
}
