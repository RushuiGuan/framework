﻿using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class WriteMethodTest : IClassFixture<MyTestHost> {
	public WriteMethodTest(MyTestHost host) {
			this.host = host;
		}

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
		private readonly MyTestHost host;

		public static IEnumerable<object[]> GetTestCases() {
			return new List<object[]> {
				new object[]{new Method("Test"){
					AccessModifier = AccessModifier.Public,
					CodeBlock = new CSharpCodeBlock("int i = 100;"),
					Override = false,
					ReturnType = DotNetType.Integer(),
				},NormalMethod.RemoveCarriageReturn(),
				},
				new object[]{new Method("Test"){
					AccessModifier = AccessModifier.Public,
					Override = false,
					ReturnType = DotNetType.Integer(),
					Parameters = new Parameter[]{
						new Parameter("a",DotNetType.Integer()),
						new Parameter("b",DotNetType.String()),
					},
				},ParameterizedMethod.RemoveCarriageReturn(),
				},
				new object[]{new Method("Test"){
					AccessModifier = AccessModifier.Public,
					Static = true,
					ReturnType = DotNetType.Void(),
				},StaticMethod.RemoveCarriageReturn(),
				},
				new object[]{new Method("Test"){
					AccessModifier = AccessModifier.Public,
					Override = true,
					ReturnType = DotNetType.Void(),
				},OverrideMethod.RemoveCarriageReturn(),
				},
				new object[]{new Method("Test"){
					AccessModifier = AccessModifier.Public,
					Virtual = true,
					ReturnType = DotNetType.Void(),
				},VirtualMethod.RemoveCarriageReturn(),
				},
			};
		}

		[Theory]
		[MemberData(nameof(GetTestCases))]
		public void Run(Method method, string expected) {
			WriteMethod writeMethod = host.Provider.GetRequiredService<WriteMethod>();
			StringWriter writer = new StringWriter();
			writer.Run(writeMethod, method);
			string actual =writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}
	}
}
