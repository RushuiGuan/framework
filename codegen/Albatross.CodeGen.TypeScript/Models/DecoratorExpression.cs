namespace Albatross.CodeGen.TypeScript.Models {
	public class Injectable {
		public Injectable(string providedIn) {
			ProvidedIn = providedIn;
		}

		public string ProvidedIn { get; set; }
	}

	public record class DecoratorExpression<T> : MethodCallExpression {
		public DecoratorExpression(T t) : base(false, name, new JsonObject<T>(t)) {
		}
	}
	
	public record class InjectableDecoratorSyntax : MethodCallExpression {
		public InjectableDecoratorSyntax(string providedIn) 
			: base(false, "@Injectable", new JsonObject<Injectable>(new Injectable(providedIn))) {
		}
	}
}
