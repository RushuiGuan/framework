using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.CSharp.Models {
	[Flags]
	public enum AccessModifier {
		None = 0,
		Public = 1,
		Private = 2,
		Protected = 4,
		Internal = 8,
	}

	public class AccessModifierElement : ICodeElement {
		public AccessModifierElement(AccessModifier accessModifier) {
			AccessModifier = accessModifier;
		}

		public AccessModifier AccessModifier { get; }

		public TextWriter Generate(TextWriter writer) {
			if (AccessModifier == AccessModifier.Internal) {
				writer.Write("internal");
			} else {
				if ((AccessModifier & AccessModifier.Public) > 0) {
					writer.Write("public");
				} else if ((AccessModifier & AccessModifier.Private) > 0) {
					writer.Write("private");
				} else if ((AccessModifier & AccessModifier.Protected) > 0) {
					writer.Write("protected");
				}
				if ((AccessModifier & AccessModifier.Internal) > 0) {
					writer.Write(" internal");
				}
			}
			return writer;
		}
	}
}