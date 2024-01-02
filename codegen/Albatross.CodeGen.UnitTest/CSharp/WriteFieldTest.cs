using Albatross.CodeGen.CSharp.Models;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class WriteFieldTest {
		public const string NormalMethod = @"public System.Int32 Test;";
		public const string StaticMethod = @"public static System.String a;";

		public static IEnumerable<object[]> GetTestCases() {
			return new List<object[]> {
				new object[]{new Field ("Test", DotNetType.Integer()){
					Modifier = AccessModifier.Public,
				},NormalMethod.RemoveCarriageReturn(),              },
				new object[]{new Field("a", DotNetType.String()){
					Modifier = AccessModifier.Public,
					Static = true,
				},StaticMethod.RemoveCarriageReturn(),        },
			};
		}

		[Theory]
		[MemberData(nameof(GetTestCases))]
		public void Run(Field field, string expected) {
			StringWriter writer = new StringWriter();
			writer.Code(field);
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}
	}
}
