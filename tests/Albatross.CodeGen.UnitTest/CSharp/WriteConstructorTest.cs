﻿using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class WriteConstructorTest : IClassFixture<MyTestHost> {
	public WriteConstructorTest(MyTestHost host) {
			this.host = host;
		}

		public const string NormalConstructor = @"public Test() {
	int i = 100;
}";

		public const string ParameterizedConstructor = @"public Test(System.Int32 @a, System.String @b) {
}";
		public const string StaticConstructor = @"static Test() {
	int i = 100;
}";
		public const string BaseConstructor = @"public Test(System.Int32 @a, System.String @b) : base(@a, @b) {
	int i = 100;
}";
		public const string ChainConstructor = @"public Test(System.Int32 @a, System.String @b) : this(@a, @b) {
	int i = 100;
}";
		private readonly MyTestHost host;

		public static IEnumerable<object[]> GetTestCases() {
			Parameter[] parameters = new Parameter[]{
						new Parameter("a",DotNetType.Integer()),
						new Parameter("b",DotNetType.String()),
					};
			return new List<object[]> {
				new object[]{new Constructor("Test"){
					AccessModifier = AccessModifier.Public,
					CodeBlock = new CodeBlock("int i = 100;"),
				},NormalConstructor.RemoveCarriageReturn(), },

				new object[]{new Constructor("Test"){
					AccessModifier = AccessModifier.Public,
					Parameters = parameters,
				}, ParameterizedConstructor.RemoveCarriageReturn(), },
				new object[]{new Constructor("Test"){
					Static = true,
					CodeBlock = new CodeBlock("int i = 100;"),
				},StaticConstructor.RemoveCarriageReturn(),             },
				new object[]{new Constructor("Test"){
					AccessModifier = AccessModifier.Public,
					Parameters = parameters,
					CodeBlock = new CodeBlock("int i = 100;"),
					BaseConstructor = new Constructor("base"){
						Parameters = parameters,
					}
				},BaseConstructor.RemoveCarriageReturn(),               },
				new object[]{new Constructor("Test"){
					AccessModifier = AccessModifier.Public,
					Parameters = parameters,
					CodeBlock = new CodeBlock("int i = 100;"),
					BaseConstructor = new Constructor("this"){
						Parameters = parameters,
					}
				}, ChainConstructor.RemoveCarriageReturn(),             },
			};
		}

		[Theory]
		[MemberData(nameof(GetTestCases))]
		public void Run(Constructor constructor, string expected) {
			WriteConstructor writeConstructor = host.Provider.GetRequiredService<WriteConstructor>();
			StringWriter writer = new StringWriter();
			writer.Run(writeConstructor, constructor);
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}
	}
}
