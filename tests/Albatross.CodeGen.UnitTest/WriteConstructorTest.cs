using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.UnitTest {
	[TestFixture(TestOf = typeof(WriteConstructor))]
	public class WriteConstructorTest : TestBase {

		public const string NormalConstructor = @"public Test() {
	int i = 100;
}";

		public const string ParameterizedConstructor = @"public Test(System.Int32 @a, System.String @b) {
}";
		public const string StaticConstructor = @"static Test() {
	int i = 100;
}";
		public const string BaseConstructor= @"public Test(System.Int32 @a, System.String @b) : base(@a, @b) {
	int i = 100;
}";
		public const string ChainConstructor = @"public Test(System.Int32 @a, System.String @b) : this(@a, @b) {
	int i = 100;
}";

		public static IEnumerable<TestCaseData> GetTestCases() {
			Parameter[] parameters = new Parameter[]{
						new Parameter{
							 Name = "a",
							 Type = DotNetType.Integer(),
						},
						new Parameter{
							 Name = "b",
							 Type = DotNetType.String(),
						},
					};
			return new TestCaseData[] {
				new TestCaseData(new Constructor{
					AccessModifier = AccessModifier.Public,
					Name = "Test",
					Body = new CodeBlock("int i = 100;"),
				}){
					ExpectedResult = NormalConstructor.RemoveCarriageReturn(),
				},
				new TestCaseData(new Constructor{
					AccessModifier = AccessModifier.Public,
					Name = "Test",
					Parameters = parameters,
				}){
					ExpectedResult = ParameterizedConstructor.RemoveCarriageReturn(),
				},
				new TestCaseData(new Constructor{
					Static = true,
					Name = "Test",
					Body = new CodeBlock("int i = 100;"),
				}){
					ExpectedResult = StaticConstructor.RemoveCarriageReturn(),
				},
				new TestCaseData(new Constructor{
					AccessModifier = AccessModifier.Public,
					Name = "Test",
					Parameters = parameters,
					Body = new CodeBlock("int i = 100;"),
					BaseConstructor = new Constructor{
						Name = "base",
						Parameters = parameters,
					}
				}){
					ExpectedResult = BaseConstructor.RemoveCarriageReturn(),
				},
				new TestCaseData(new Constructor{
					AccessModifier = AccessModifier.Public,
					Name = "Test",
					Parameters = parameters,
					Body = new CodeBlock("int i = 100;"),
					BaseConstructor = new Constructor{
						Name = "this",
						Parameters = parameters,
					}
				}){
					ExpectedResult = ChainConstructor.RemoveCarriageReturn(),
				},
			};
		}


		[TestCaseSource(nameof(GetTestCases))]
		public string Run(Constructor constructor) {
			WriteConstructor writeConstructor = provider.GetRequiredService<WriteConstructor>();
			StringWriter writer = new StringWriter();
			writer.Run(writeConstructor, constructor);
			return writer.ToString().RemoveCarriageReturn();
		}
	}
}
