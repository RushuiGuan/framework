using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class AccessModifierSyntax : ICodeElement {
		public required SyntaxTree SyntaxTree { get; init; }
		public AccessModifier AccessModifier { get; }
		internal AccessModifierSyntax(AccessModifier accessModifier) {
			AccessModifier = accessModifier;
		}

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
