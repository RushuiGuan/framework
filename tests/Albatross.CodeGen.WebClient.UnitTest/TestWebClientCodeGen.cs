using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Albatross.CodeGen.WebClient.UnitTest {
	public class TestWebClientCodeGen : IClassFixture<WebClientTestHost> {
		private readonly WebClientTestHost host;

		public TestWebClientCodeGen(WebClientTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestHttpGet() {
			using (var scope = host.Create()) {
				Type type = typeof(TestHttpGetController);
				ConvertApiControllerToCSharpClass handle = scope.Get<ConvertApiControllerToCSharpClass>();
				Class converted = handle.Convert(type);
				StringBuilder sb = new StringBuilder();
				using (StringWriter writer = new StringWriter(sb)) {
					writer.Code(converted);
				}
				string expectedFile = Path.Join(this.GetType().GetAssemblyLocation(), "TestHttpGetProxyService.expected.cs");
				using (StreamReader reader = new StreamReader(expectedFile)) {
					string expected = reader.ReadToEnd();
					Assert.Equal(expected, sb.ToString());
				}
			}
		}

		[Fact]
		public void TestHttpPost() {
			using (var scope = host.Create()) {
				Type type = typeof(TestHttpPostController);
				ConvertApiControllerToCSharpClass handle = scope.Get<ConvertApiControllerToCSharpClass>();
				Class converted = handle.Convert(type);
				StringBuilder sb = new StringBuilder();
				using (StringWriter writer = new StringWriter(sb)) {
					writer.Code(converted);
				}
				string expectedFile = Path.Join(this.GetType().GetAssemblyLocation(), "TestHttpPostProxyService.expected.cs");
				using (StreamReader reader = new StreamReader(expectedFile)) {
					string expected = reader.ReadToEnd();
					Assert.Equal(expected, sb.ToString());
				}
			}
		}

		[Fact]
		public void TestHttpDelete() {
			using (var scope = host.Create()) {
				Type type = typeof(TestHttpDeleteController);
				ConvertApiControllerToCSharpClass handle = scope.Get<ConvertApiControllerToCSharpClass>();
				Class converted = handle.Convert(type);
				StringBuilder sb = new StringBuilder();
				using (StringWriter writer = new StringWriter(sb)) {
					writer.Code(converted);
				}
				string expectedFile = Path.Join(this.GetType().GetAssemblyLocation(), "TestHttpDeleteProxyService.expected.cs");
				using (StreamReader reader = new StreamReader(expectedFile)) {
					string expected = reader.ReadToEnd();
					Assert.Equal(expected, sb.ToString());
				}
			}
		}

		[Theory]
		[InlineData("{name}", "name")]
		[InlineData("{name}/{id}", "name id")]
		[InlineData("api/{name}", "name")]
		[InlineData("api/{name}/{id}", "name id")]
		[InlineData("{name}/api/{id}", "name id")]
		[InlineData("{name}/{id}/api", "name id")]
		[InlineData("{**name}", "name")]
		[InlineData("api/{id}/{**name}", "id name")]
		public void TestRouteRegex(string input, string expected) {
			List<string> list = new List<string>();
			foreach (Match match in ConvertApiControllerToCSharpClass.actionRouteRegex.Matches(input)) {
				list.Add(match.Groups[1].Value);
			}
			Assert.Equal(expected, string.Join(' ', list));
		}
	}
}
