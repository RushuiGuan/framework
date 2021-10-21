using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.Config;
using Albatross.IAM.Api;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Albatross.CodeGen.WebClient.UnitTest {

	public class GeneralTest : IClassFixture<WebClientTestHost> {
		private readonly WebClientTestHost host;

		public GeneralTest(WebClientTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestGroupController() {
			using (var scope = host.Create()) {
				Type type = typeof(GroupController);
				ConvertApiControllerToCSharpClass handle = scope.Get<ConvertApiControllerToCSharpClass>();
				Class converted = handle.Convert(type);
				StringBuilder sb = new StringBuilder();
				using (StringWriter writer1 = new StringWriter(sb)) {
					writer1.Code(converted);
				}
				string result = sb.ToString();
				string expectedFile = Path.Join(this.GetType().GetAssemblyLocation(), "GroupClientService.expected.cs");
				using (StreamReader reader = new StreamReader(expectedFile)) {
					string expected = reader.ReadToEnd();
					Assert.Equal(expected, result);
				}
			}
		}

		[Fact]
		public void TestValueController() {
			using (var scope = host.Create()) {
				Type type = typeof(ValueController);
				ConvertApiControllerToCSharpClass handle = scope.Get<ConvertApiControllerToCSharpClass>();
				Class converted = handle.Convert(type);
				StringBuilder sb = new StringBuilder();
				using (StringWriter writer1 = new StringWriter(sb)) {
					writer1.Code(converted);
				}
				string result = sb.ToString();

				string expectedFile = Path.Join(this.GetType().GetAssemblyLocation(), "ValueClientService.expected.cs");
				using (StreamReader reader = new StreamReader(expectedFile)) {
					string expected = reader.ReadToEnd();
					Assert.Equal(expected, result);
				}
			}
		}
	}
}
