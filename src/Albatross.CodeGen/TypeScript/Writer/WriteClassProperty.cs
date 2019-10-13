using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer
{
    public class WriteClassProperty : CodeGeneratorBase<Property>
    {
        WriteTypeScriptType writeTypeScriptType;

        public WriteClassProperty(WriteTypeScriptType writeTypeScriptType) {
            this.writeTypeScriptType = writeTypeScriptType;
        }

        public override void Run(TextWriter writer, Property t)
        {
            writer.Tab().Append(t.Name).Append(": ");
            writer.Run(writeTypeScriptType, t.Type).Append(";");
        }
    }
}
