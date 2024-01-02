using Albatross.CodeGen.CSharp.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class WriteMethodCallTest {
		public const string NormalMethodCall = @"Test()";
		public const string ParameterizedMethodCall1= @"Test(@a, @b)";
		public const string ParameterizedMethodCall2 = @"Test(1, 2)";
		public const string ParameterizedMethodCall3 = @"Test(""a"", ""b"")";
		public const string AsyncMethodCall = @"await Test()";

		public static IEnumerable<object[]> GetTestCases() {
			return new List<object[]> {
				new object[]{
					new MethodCall("Test"),
					NormalMethodCall.RemoveCarriageReturn(),
				},
				new object[]{
					new MethodCall("Test"){
						Parameters = new ICodeElement[]{
							new Variable("a", true),
							new Variable("b", true),
						},
					},
					ParameterizedMethodCall1.RemoveCarriageReturn(),
				},
				new object[]{
					new MethodCall("Test"){
						Parameters = new ICodeElement[]{
							new Literal(1),
							new Literal(2),
						},
					},
					ParameterizedMethodCall2.RemoveCarriageReturn(),
				},
				new object[]{
					new MethodCall("Test"){
						Parameters = new ICodeElement[]{
							new StringLiteral("a"),
							new StringLiteral("b"),
						},
					},
					ParameterizedMethodCall3.RemoveCarriageReturn(),
				},
				new object[]{
					new MethodCall("Test"){
						Await = true
					},
					AsyncMethodCall.RemoveCarriageReturn(),
				}
			};
		}

		[Theory]
		[MemberData(nameof(GetTestCases))]
		public void Run(MethodCall method, string expected) {
			var writer = new StringWriter();
			writer.Code(method);
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}
	}
}
