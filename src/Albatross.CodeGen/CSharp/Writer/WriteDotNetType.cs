using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.CSharp.Writer
{
	public class WriteDotNetType: CodeGeneratorBase<DotNetType> {
		public override void Run(TextWriter writer, DotNetType type) {
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
