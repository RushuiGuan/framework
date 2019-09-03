
using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.UnitTest {
	[TestFixture(TestOf = typeof(WriteCSharpClass))]
	public class WriteClassTest : TestBase {

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

		public static IEnumerable<TestCaseData> GetTestCases() {
			return new TestCaseData[] {
				new TestCaseData(new Class{
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
				}){
					ExpectedResult = ClassA.RemoveCarriageReturn(),
				},
				new TestCaseData(new Class{
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
				}){
					ExpectedResult = ClassB.RemoveCarriageReturn(),
				},
			};
		}


		[TestCaseSource(nameof(GetTestCases))]
		public string Run(Class @class) {
			WriteCSharpClass writeClass = provider.GetRequiredService<WriteCSharpClass>();
			StringWriter writer = new StringWriter();
			writer.Run(writeClass, @class);
			return writer.ToString().RemoveCarriageReturn();
		}
	}
}