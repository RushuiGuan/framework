using Albatross.CodeGen.Python.Models;

namespace Albatross.CodeGen.WebClient.Python {
	public class RestApiMethod : Method {
		public RestApiMethod(string name, string relativeUrl) : base(name) {
			HeaderSetup = new CompositeModuleCodeBlock {
				new Assignment("headers", new Literal("{}"))
			};

			CodeBlock.AddLine(HeaderSetup);
			CodeBlock.AddLine(new Assignment("url", new StringInteporation(new Literal("self.endpoint"),
				new Literal($"{this.Name}.BASE_URL"), new StringLiteral(relativeUrl))));


			RequestCall = new MethodCall("requests.get", new Variable("url")) {
				Module = My.Modules.Requests,
			};
			CodeBlock.AddLine(new Assignment("response", RequestCall));
			CodeBlock.AddLine(new MethodCall("response.raise_for_status"));
		}
		public MethodCall RequestCall { get; set; }
		public CompositeModuleCodeElement HeaderSetup { get; set; }
	}
}
