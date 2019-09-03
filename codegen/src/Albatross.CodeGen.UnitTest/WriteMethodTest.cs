using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.UnitTest {
	[TestFixture(TestOf = typeof(WriteMethod))]
	public class WriteMethodTest : TestBase {

		public const string NormalMethod = @"public System.Int32 Test() {
	int i = 100;
}";

		public const string ParameterizedMethod = @"public System.Int32 Test(System.Int32 @a, System.String @b) {
}";
		public const string StaticMethod = @"public static void Test() {
}";
		public const string OverrideMethod = @"public override void Test() {
}";
		public const string VirtualMethod = @"public virtual void Test() {
}";

		public static IEnumerable<TestCaseData> GetTestCases() {
			return new TestCaseData[] {
				new TestCaseData(new Method{
					AccessModifier = AccessModifier.Public,
					Name = "Test",
					Body = new CodeBlock("int i = 100;"),
					Override = false,
					ReturnType = DotNetType.Integer(),
				}){
					ExpectedResult = NormalMethod.RemoveCarriageReturn(),
				},
				new TestCaseData(new Method{
					AccessModifier = AccessModifier.Public,
					Name = "Test",
					Override = false,
					ReturnType = DotNetType.Integer(),
					Parameters = new Parameter[]{
						new Parameter{
							 Name = "a",
							 Type = DotNetType.Integer(),
						},
						new Parameter{
							 Name = "b",
							 Type = DotNetType.String(),
						},
					},
				}){
					ExpectedResult = ParameterizedMethod.RemoveCarriageReturn(),
				},
				new TestCaseData(new Method{
					AccessModifier = AccessModifier.Public,
					Name = "Test",
					Static = true,
					ReturnType = DotNetType.Void(),
				}){
					ExpectedResult = StaticMethod.RemoveCarriageReturn(),
				},
				new TestCaseData(new Method{
					AccessModifier = AccessModifier.Public,
					Name = "Test",
					Override = true,
					ReturnType = DotNetType.Void(),
				}){
					ExpectedResult = OverrideMethod.RemoveCarriageReturn(),
				},
				new TestCaseData(new Method{
					AccessModifier = AccessModifier.Public,
					Name = "Test",
					Virtual = true,
					ReturnType = DotNetType.Void(),
				}){
					ExpectedResult = VirtualMethod.RemoveCarriageReturn(),
				},
			};
		}


		[TestCaseSource(nameof(GetTestCases))]
		public string Run(Method method) {
			WriteMethod writeMethod = provider.GetRequiredService<WriteMethod>();
			StringWriter writer = new StringWriter();
			writer.Run(writeMethod, method);
			return writer.ToString().RemoveCarriageReturn();
		}
	}
}
