using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.UnitTest {
	[TestFixture(TestOf = typeof(WriteField))]
	public class WriteFieldTest : TestBase {

		public const string NormalMethod = @"public System.Int32 Test;";
		public const string StaticMethod = @"public static System.String a;";

		public static IEnumerable<TestCaseData> GetTestCases() {
			return new TestCaseData[] {
				new TestCaseData(new Field{
					Modifier = AccessModifier.Public,
					Name = "Test",
					Type = DotNetType.Integer(),
				}){
					ExpectedResult = NormalMethod.RemoveCarriageReturn(),
				},
				new TestCaseData(new Field{
					Modifier = AccessModifier.Public,
					Name = "a",
					Static = true,
					Type = DotNetType.String(),
				}){
					ExpectedResult = StaticMethod.RemoveCarriageReturn(),
				},
			};
		}


		[TestCaseSource(nameof(GetTestCases))]
		public string Run(Field field) {
			WriteField writeField = provider.GetRequiredService<WriteField>();
			StringWriter writer = new StringWriter();
			writer.Run(writeField, field);
			return writer.ToString().RemoveCarriageReturn();
		}
	}
}
