using Albatross.CodeGen.CSharp.Model;
using Albatross.Config;
using Albatross.Host.NUnit;
using Albatross.IAM.Api;
using Albatross.Test.Api;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.WebClient.UnitTest {
	[TestFixture]
	public class GeneralTest :TestBase<TestUnitOfWork>{
        public override void RegisterPackages(IServiceCollection svc) {
            svc.AddDefaultCodeGen().AddCodeGen(this.GetType().Assembly);
            svc.AddTransient<ConvertApiControllerToCSharpClass>();

        }

		[Test]
		public void TestGroupController() {
			using (var unitOfWork = NewUnitOfWork()) {
				Type type = typeof(GroupController);
				ConvertApiControllerToCSharpClass handle = unitOfWork.Get<ConvertApiControllerToCSharpClass>();
				Class converted = handle.Convert(type);
				var writer = unitOfWork.Get<Albatross.CodeGen.CSharp.Writer.WriteCSharpClass>();
				StringBuilder sb = new StringBuilder();
				using(StringWriter writer1 = new StringWriter(sb)) {
					writer.Run(writer1, converted);
				}
				string result = sb.ToString();
                string file = @"C:\git\app\webclient\src\Albatross.CodeGen.WebClient.UnitTest\GroupClientService.actual.cs";
                using (StreamWriter streamWriter = new StreamWriter(file)) {
                    streamWriter.Write(result);
                }

                string expectedFile = Path.Join(GetType().GetAssemblyLocation(), "GroupClientService.expected.cs");
                using (StreamReader reader = new StreamReader(expectedFile)) {
                    string expected = reader.ReadToEnd();
                    Assert.AreEqual(expected, result);
                }
            }
		}

		[Test]
		public void TestValueController() {
			using (var unitOfWork = NewUnitOfWork()) {
				Type type = typeof(ValueController);
				ConvertApiControllerToCSharpClass handle = unitOfWork.Get<ConvertApiControllerToCSharpClass>();
				Class converted = handle.Convert(type);
				var writer = unitOfWork.Get<Albatross.CodeGen.CSharp.Writer.WriteCSharpClass>();
				StringBuilder sb = new StringBuilder();
				using (StringWriter writer1 = new StringWriter(sb)) {
					writer.Run(writer1, converted);
				}
				string result = sb.ToString();
				string file = @"C:\git\app\webclient\src\Albatross.CodeGen.WebClient.UnitTest\ValueClientService.actual.cs";
				using (StreamWriter streamWriter = new StreamWriter(file)) {
					streamWriter.Write(result);
				}

				string expectedFile = Path.Join(this.GetType().GetAssemblyLocation(), "ValueClientService.expected.cs");
				using (StreamReader reader = new StreamReader(expectedFile)) {
					string expected = reader.ReadToEnd();
					Assert.AreEqual(expected, result);
				}
			}
		}
	}
}
