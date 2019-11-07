using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Albatross.CodeGen.UnitTest {
	public class WriteClassTest : IClassFixture<MyTestHost> {
		public WriteClassTest(MyTestHost host) {
			this.host = host;
		}

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
		private readonly MyTestHost host;

		public static IEnumerable<object[]> GetTestCases() {
			return new List<object[]> {
				new object[]{
					new Class{
						AccessModifier = AccessModifier.Public,
						Name ="ClassA",
						Namespace = "Albatross.Sample",
						Imports = new string[]{"System", "System.IO",},
						Properties = new Property[]{
							new Property{
								Name = "P1",
								Type = DotNetType.String(),
								CanRead = true,
								CanWrite = true,
								Modifier = AccessModifier.Public,
							}
						},
					},
					ClassA.RemoveCarriageReturn(),
				},
				new object[]{
					new Class{
						AccessModifier = AccessModifier.Public,
						Name ="ClassB",
						Static = true,
						Partial = true,
						Namespace = "Albatross.Sample",
						Properties = new Property[]{
							new Property{
								Name = "P1",
								Type = DotNetType.String(),
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
			WriteCSharpClass writeClass = host.Provider.GetRequiredService<WriteCSharpClass>();
			StringWriter writer = new StringWriter();
			writer.Run(writeClass, @class);
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}
	}
}