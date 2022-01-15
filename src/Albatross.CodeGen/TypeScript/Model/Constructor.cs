using Albatross.CodeGen.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Constructor :Method {
		public Constructor(bool hasBaseClass) : base("constructor") {
			AccessModifier = AccessModifier.Public;
			Async = false;
			var body = new CodeBlock();
			if (hasBaseClass) {
				body.Add("super();");
			}
			this.Body = body;
		}
	}
}
