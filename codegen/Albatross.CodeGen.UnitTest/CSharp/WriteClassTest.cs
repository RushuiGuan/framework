using Albatross.CodeGen.CSharp.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class WriteClassTest {
		public const string ClassA = @"using System;
using System.IO;

namespace Albatross.Sample {
	public class ClassA {
		public System.String P1 {
			get;set;
		}
	}
}";
		public const string ClassB = @"namespace Albatross.Sample {
	public static partial class ClassB {
		public System.String P1 {
			get;set;
		}
	}
}";
		public const string StaticMethod = @"public static System.String a;";

		public static IEnumerable<object[]> GetTestCases() {
			return new List<object[]> {
				new object[]{
					new Class("ClassA"){
						AccessModifier = AccessModifier.Public,
						Namespace = "Albatross.Sample",
						Imports = new string[]{"System", "System.IO",},
						Properties = new Property[]{
							new Property("P1", DotNetType.String()){
								CanRead = true,
								CanWrite = true,
								Modifier = AccessModifier.Public,
							}
						},
					},
					ClassA.RemoveCarriageReturn(),
				},
				new object[]{
					new Class("ClassB"){
						AccessModifier = AccessModifier.Public,
						Static = true,
						Partial = true,
						Namespace = "Albatross.Sample",
						Properties = new Property[]{
							new Property("P1", DotNetType.String()){
								CanRead = true,
								CanWrite = true,
								Modifier = AccessModifier.Public,
							}
						},
					},
					ClassB.RemoveCarriageReturn(),
				},
			};
		}


		[Theory]
		[MemberData(nameof(GetTestCases))]
		public void Run(Class @class, string expected) {
			StringWriter writer = new StringWriter();
			writer.Code(@class);
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}
	}
}