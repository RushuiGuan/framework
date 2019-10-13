using Albatross.CodeGen;
using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer
{
    public class WriteInterface : CodeGeneratorBase<Interface>
    {
        WriteClassProperty writeProperty;
        public WriteInterface(WriteClassProperty writeProperty)
        {
            this.writeProperty = writeProperty;
        }

        public override void Run(TextWriter writer, Interface t)
        {
            if (t.Export)
            {
                writer.Append("export ");
            }
            writer.Append("interface ").Append(t.Name).Append("{");
            if (t.Properties != null)
            {
                foreach (var property in t.Properties)
                {
                    writer.Run(writeProperty, property).WriteLine();
                }
            }
            writer.Append("}");
        }
    }
}
