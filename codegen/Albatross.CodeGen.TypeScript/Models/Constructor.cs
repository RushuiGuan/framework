
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Models {
	public class Constructor :Method {
		public Constructor() : base("constructor") {
			AccessModifier = AccessModifier.Public;
			Async = false;
			var body = new CodeBlock();
			this.Body = body;
		}
	}
}
