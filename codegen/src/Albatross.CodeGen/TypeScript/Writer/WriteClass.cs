using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer
{
    public class WriteClass : CodeGeneratorBase<Class>
    {
        WriteClassProperty writeProperty;
        public WriteClass(WriteClassProperty writeProperty)
        {
            this.writeProperty = writeProperty;
        }

        public override void Run(TextWriter writer, Class t)
        {
            if (t.Export)
            {
                writer.Append("export ");
            }
            writer.Append("class ").Append(t.Name).AppendLine("{");
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
