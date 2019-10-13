using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.CSharp.Writer {
	public class WriteAccessModifier: CodeGeneratorBase<AccessModifier> {
		public override void Run(TextWriter writer, AccessModifier accessModifier) {
			if (accessModifier == AccessModifier.Internal) {
				writer.Write("internal");
			} else {
				if ((accessModifier & AccessModifier.Public) > 0) {
					writer.Write("public");
				} else if ((accessModifier & AccessModifier.Private) > 0) {
					writer.Write("private");
				} else if ((accessModifier & AccessModifier.Protected) > 0) {
					writer.Write("protected");
				}
				if ((accessModifier & AccessModifier.Internal) > 0) {
					writer.Write(" internal");
				}
			}
		}
	}
}
