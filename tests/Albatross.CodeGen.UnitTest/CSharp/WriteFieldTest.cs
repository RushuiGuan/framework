using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class WriteFieldTest : IClassFixture<MyTestHost> {
		public WriteFieldTest(MyTestHost host) {
			this.host = host;
		}

		public const string NormalMethod = @"public System.Int32 Test;";
		public const string StaticMethod = @"public static System.String a;";
		private readonly MyTestHost host;

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
			WriteField writeField = host.Provider.GetRequiredService<WriteField>();
			StringWriter writer = new StringWriter();
			writer.Run(writeField, field);
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}
	}
}
