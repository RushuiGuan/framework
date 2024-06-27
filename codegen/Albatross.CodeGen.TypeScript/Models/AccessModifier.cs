using System;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	[Flags]
	public enum AccessModifier {
		None = 0,
		Public = 1,
		Private = 2,
		Protected = 4,
	}

	public class AccessModifierElement : ICodeElement {
		public AccessModifierElement(AccessModifier accessModifier) {
			AccessModifier = accessModifier;
		}
		public AccessModifier AccessModifier { get; }

		public TextWriter Generate(TextWriter writer) {
			switch (AccessModifier) {
				case AccessModifier.Public:
					writer.Write("public ");
					break;
				case AccessModifier.Private:
					writer.Write("private ");
					break;
				case AccessModifier.Protected:
					writer.Write("protected ");
					break;
			}
			return writer;
		}
	}
}
