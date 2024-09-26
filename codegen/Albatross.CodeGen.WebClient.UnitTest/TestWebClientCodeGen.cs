using Albatross.CodeGen.WebClient.CSharpOld;
using Albatross.CodeGen.WebClient.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Albatross.CodeGen.WebClient.UnitTest {
	public class TestWebClientCodeGen : IClassFixture<WebClientTestHost> {
		private readonly WebClientTestHost host;

		public TestWebClientCodeGen(WebClientTestHost host) {
			this.host = host;
		}

		/// <summary>
		/// aspnet route use ** for a catch all parameter.  It will set the parameter by matching the rest of the url
		/// ** should always be followed by the variable name
		/// </summary>
		[Theory]
		[InlineData("{name}", "name")]
		[InlineData("{name}/{id}", "name id")]
		[InlineData("api/{name}", "name")]
		[InlineData("api/{name}/{id}", "name id")]
		[InlineData("{name}/api/{id}", "name id")]
		[InlineData("{name}/{id}/api", "name id")]
		[InlineData("{**name}", "name")]
		[InlineData("{**_name}", "_name")]
		[InlineData("{**name9}", "name9")]
		[InlineData("{**nam9e}", "nam9e")]
		[InlineData("api/{id}/{**name}", "id name")]
		[InlineData("api/{id}/{*name}", "id name")]
		public void TestRouteRegex(string input, string expected) {
			var list = new List<string>();
			foreach (Match match in ConvertApiControllerToCSharpClass.ActionRouteRegex.Matches(input)) {
				list.Add(match.Groups[2].Value);
			}
			Assert.Equal(expected, string.Join(' ', list));
		}


		[Theory]
		[InlineData("snapshot/{**name}", @"string path = $""{ControllerPath}/snapshot/{name}"";")]
		[InlineData("snapshot/{*name}", @"string path = $""{ControllerPath}/snapshot/{name}"";")]
		[InlineData("snapshot", @"string path = $""{ControllerPath}/snapshot"";")]
		[InlineData("snapshot/{tradeDate}", @"string path = $""{ControllerPath}/snapshot/{tradeDate:yyyy-MM-dd}"";")]
		[InlineData("snapshot/{date}", @"string path = $""{ControllerPath}/snapshot/{date:yyyy-MM-dd}"";")]
		[InlineData("snapshot/{tradeDate}/red", @"string path = $""{ControllerPath}/snapshot/{tradeDate:yyyy-MM-dd}/red"";")]
		[InlineData("snapshot/{tradeDate}/{**name}", @"string path = $""{ControllerPath}/snapshot/{tradeDate:yyyy-MM-dd}/{name}"";")]
		[InlineData("snapshot/{tradeDate}/{id}", @"string path = $""{ControllerPath}/snapshot/{tradeDate:yyyy-MM-dd}/{id}"";")]
		[InlineData("snapshot/{test_tradeDate}", @"string path = $""{ControllerPath}/snapshot/{test_tradeDate:yyyy-MM-dd}"";")]
		[InlineData("{tradeDate}", @"string path = $""{ControllerPath}/{tradeDate:yyyy-MM-dd}"";")]
		[InlineData("{tradeDate1}/{tradeDate2}", @"string path = $""{ControllerPath}/{tradeDate1:yyyy-MM-dd}/{tradeDate2:yyyy-MM-dd}"";")]
		[InlineData("snapshot/test{tradeDate}", @"string path = $""{ControllerPath}/snapshot/test{tradeDate:yyyy-MM-dd}"";")]
		[InlineData("snapshot/test{tradeDate}test2{xx}", @"string path = $""{ControllerPath}/snapshot/test{tradeDate:yyyy-MM-dd}test2{xx}"";")]
		[InlineData("snapshot/wacky{tradeDate}doodle{xx}string", @"string path = $""{ControllerPath}/snapshot/wacky{tradeDate:yyyy-MM-dd}doodle{xx}string"";")]
		public void TestAddCSharpRoutingParam(string input, string expected) {
			var writer = new StringWriter();
			var codeElement = new AddCSharpRouteUrl(input);
			codeElement.Generate(writer);
			writer.Flush();
			string result = writer.ToString();
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("{**name}", true, "", "**", "name")]
		[InlineData("{**name9}", true, "", "**", "name9")]
		[InlineData("{**tradeDate}", true,"",  "**", "tradeDate")]
		[InlineData("{_tradeDate}", true, "_tradeDate", "", "")]
		[InlineData("{tradeDate}", true, "tradeDate", "", "")]
		[InlineData("{tradeDate1}", true, "tradeDate1", "", "")]
		[InlineData("{date}", true, "date", "", "")]
		[InlineData("{date1}", true, "date1", "", "")]
		public void TestRoutingParamRegex(string input, bool success, string dateParam, string catchAll, string name) {
			var match = AddCSharpRouteUrl.ParamRegex.Match(input);
			Assert.Equal(success, match.Success);
			if (success) {
				Assert.Equal(dateParam, match.Groups[1].Value);
				Assert.Equal(catchAll, match.Groups[3].Value);
				Assert.Equal(name, match.Groups[4].Value);
			}
		}
	}
}
