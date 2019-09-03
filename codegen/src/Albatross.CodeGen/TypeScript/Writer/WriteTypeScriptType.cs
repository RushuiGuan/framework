using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer
{
    public class WriteTypeScriptType : CodeGeneratorBase<TypeScriptType>
    {
        public override void Run(TextWriter writer, TypeScriptType t)
        {
            if (t.IsArray)
            {
                writer.Write($"{t.Name}[]");
            }
            else
            {
                writer.Write(t.Name);
            }
        }
    }
}
