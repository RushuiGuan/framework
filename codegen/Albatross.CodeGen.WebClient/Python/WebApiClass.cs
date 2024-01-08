using Albatross.CodeGen.Python.Models;

namespace Albatross.CodeGen.WebClient.Python {
	public class WebApiClass : Class {
		public string BaseUrl { get; set; }
		public WebApiClass(string name, string baseUrl) : base(name) {
			BaseUrl = baseUrl;
		}
		public override void Build() {
			base.Build();
			AddField(new Field("BASE_URL", My.Types.String(), new StringLiteral(BaseUrl)) { Static = true });
			Constructor = new Constructor();
			Constructor.Parameters.Add(new Variable("endpoint", My.Types.String()));
			Constructor.InitFields.Add(new Field("endpoint", My.Types.String(), new Variable("endpoint")));
			
			var method = new Method("get_fields") {
				ReturnType = My.Types.List(true),
			};
			method.CodeBlock.AddLine(new Assignment("response", new MethodCall("requests.get", new StringLiteral("abcdefg"))));
			method.CodeBlock.AddLine(new Assignment("data", new MethodCall("json.load", new MethodCall("response.json"))));
			method.CodeBlock.AddLine(new MethodCall("json.loads", new MethodCall("response.json"), new Assignment("object_hook", new Literal("lambda d: SimpleNamespace(**d)"))));
			AddMethod(method);
		}
	}
}
