
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Models {
	public class ConstructorDeclaration :MethodDeclaration {
		public ConstructorDeclaration() : base("constructor") {
			AccessModifier = AccessModifier.Public;
			Async = false;
			var body = new CodeBlock();
			this.Body = body;
		}
	}
}
