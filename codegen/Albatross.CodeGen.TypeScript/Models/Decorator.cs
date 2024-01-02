
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class Injectable {
		public Injectable(string providedIn) {
			ProvidedIn = providedIn;
		}

		public string ProvidedIn { get; set; }
	}

	public class Decorator<T> : MethodCall {
		public Decorator(string name, T t) : base(false, name, new JsonObject<T>(t)) {
		}
	}
	
	public class InjectableDecorator : MethodCall {
		public InjectableDecorator(string providedIn) 
			: base(false, "@Injectable", new JsonObject<Injectable>(new Injectable(providedIn))) {
		}
	}
}
