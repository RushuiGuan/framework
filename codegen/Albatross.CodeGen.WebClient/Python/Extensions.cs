using Albatross.CodeGen.Python.Models;

namespace Albatross.CodeGen.WebClient.Python {
	public static class Extensions {
		public static RestApiMethod UsePost(this RestApiMethod method) {
			method.RequestCall.Method.Name = "requests.post";
			return method;
		}

		public static RestApiMethod UseGet(this RestApiMethod method) {
			method.RequestCall.Method.Name = "requests.get";
			return method;
		}

		public static RestApiMethod UsePatch(this RestApiMethod method) {
			method.RequestCall.Method.Name = "requests.patch";
			return method;
		}

		public static RestApiMethod UseDelete(this RestApiMethod method) {
			method.RequestCall.Method.Name = "requests.delete";
			return method;
		}

		public static RestApiMethod UsePut(this RestApiMethod method) {
			method.RequestCall.Method.Name = "requests.put";
			return method;
		}

		public static RestApiMethod UseQueryParam(this RestApiMethod method, string name) {
			method.Parameters.Add(new Variable(name));
			method.RequestCall.AddParameter(new Assignment("params", new Variable(name)));
			return method;
		}

		public static RestApiMethod UseJsonData(this RestApiMethod method, string name) {
			method.Parameters.Add(new Variable(name));
			method.RequestCall.AddParameter(new Assignment("data", new MethodCall("json.dumps", new Variable(name)) { 
				Module = My.Modules.Json
			}));
			method.RequestCall.AddParameter(new Assignment("headers", new Variable("headers")));
			method.HeaderSetup.AddLine(new Assignment("headers['Content-type']", new StringLiteral("application/json")));
			return method;
		}

		public static RestApiMethod ReturnDataFrame(this RestApiMethod method) {
			method.CodeBlock.AddLine(new Assignment("data", new MethodCall("pandas.json_normalize", new MethodCall("response.json")) {
				Module = My.Modules.Pandas
			}));
			method.CodeBlock.Add(new Return(new Variable("data")));
			method.ReturnType = My.Types.DataFrame();
			return method;
		}
		public static RestApiMethod HasReturnType(this RestApiMethod method, PythonType type) {
			method.ReturnType = type;
			return method;
		}
	}
}
