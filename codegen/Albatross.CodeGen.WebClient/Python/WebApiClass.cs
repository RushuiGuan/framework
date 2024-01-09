using Albatross.CodeGen.Python.Models;

namespace Albatross.CodeGen.WebClient.Python {
	public class WebApiClass : Class {
		public string BaseUrl { get; set; }
		public WebApiClass(string name, string baseUrl) : base(name) {
			BaseUrl = baseUrl;
		}
		void BuildBaseUrl() => AddField(new Field("BASE_URL", My.Types.String(), new StringLiteral(BaseUrl)) { Static = true });
		void BuildConstructor() {
			Constructor = new Constructor();
			Constructor.Parameters.Add(new Variable("endpoint", My.Types.String()));
			Constructor.InitFields.Add(new Field("endpoint", My.Types.String(), new Variable("endpoint")));
		}

		RestApiMethod CreateMethod(string name, string relativeUrl) {
			var method = new RestApiMethod(name, relativeUrl);
			return method;
		}

		public override void Build() {
			base.Build();
			BuildBaseUrl();
			BuildConstructor();
			var method = CreateMethod("get_fields", "/fields");
			method.UsePost().UseQueryParam("query").UseJsonData("data").ReturnDataFrame();
			AddMethod(method);
		}
	}
}
