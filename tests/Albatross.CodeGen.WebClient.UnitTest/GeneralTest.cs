using Albatross.CodeGen.CSharp.Model;
using Albatross.Config;
using Albatross.IAM.Api;
using System;
using System.IO;
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
				var writer = scope.Get<Albatross.CodeGen.CSharp.Writer.WriteCSharpClass>();
				StringBuilder sb = new StringBuilder();
				using (StringWriter writer1 = new StringWriter(sb)) {
					writer.Run(writer1, converted);
				}
				string result = sb.ToString();
				using var stream = this.GetType().Assembly.GetManifestResourceStream("Albatross.CodeGen.WebClient.UnitTest.GroupClientService.expected.cs");
				string expected = new StreamReader(stream).ReadToEnd().Replace("\r", null).Replace("\n", null).Replace("\t", null);
				string actualFile = Path.Join(GetType().GetAssemblyLocation(), "GroupClientService.actual.cs");
				using (StreamWriter writer2 = new StreamWriter(actualFile)) {
					writer2.Write(result);
				}
				result = result.Replace("\r", null).Replace("\n", null).Replace("\t", null);
				Assert.Equal(expected, result);
			}
		}

		[Fact(Skip = "use open api instead")]
		public void TestValueController() {
			using (var scope = host.Create()) {
				Type type = typeof(ValueController);
				ConvertApiControllerToCSharpClass handle = scope.Get<ConvertApiControllerToCSharpClass>();
				Class converted = handle.Convert(type);
				var writer = scope.Get<Albatross.CodeGen.CSharp.Writer.WriteCSharpClass>();
				StringBuilder sb = new StringBuilder();
				using (StringWriter writer1 = new StringWriter(sb)) {
					writer.Run(writer1, converted);
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
