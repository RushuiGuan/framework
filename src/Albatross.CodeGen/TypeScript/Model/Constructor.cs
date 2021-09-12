using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Constructor :Method {
		public Constructor() : base("constructor") {
			ReturnType = TypeScriptType.Void();
			AccessModifier = AccessModifier.Public;
			Async = false;
		}
		public Method BaseConstructor { get; set; }
	}
}
