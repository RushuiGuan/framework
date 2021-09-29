using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteTypeScriptProperty : CodeGeneratorBase<Property>
    {
        WriteTypeScriptType writeTypeScriptType;

        public WriteTypeScriptProperty(WriteTypeScriptType writeTypeScriptType) {
            this.writeTypeScriptType = writeTypeScriptType;
        }

		public override void Run(TextWriter writer, Property t) {
			writer.Append(t.Name);
			if (t.Optional) { writer.Append("?"); }
			writer.Append(": ");
			writer.Run(writeTypeScriptType, t.Type).Append(";").WriteLine();
		}
    }
}
