using System;
using Albatross.CodeGen.CSharp.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Albatross.CodeGen.UnitTest {
	public class ConvertTypeToDotNetTypeTest {

		public static IEnumerable<object[]> GetTestData() {
			return new List<object[]> {
				new object[]{typeof(int), new DotNetType("System.Int32")},
				new object[]{typeof(string), new DotNetType("System.String")},
				new object[]{typeof(DateTime), new DotNetType("System.DateTime")},
				new object[]{typeof(Guid), new DotNetType("System.Guid")},
				new object[]{typeof(ConvertTypeToDotNetTypeTest), new DotNetType("Albatross.CodeGen.UnitTest.ConvertTypeToDotNetTypeTest")},
				new object[]{typeof(int[]), new DotNetType("System.Int32", true, false, null)},
				new object[]{typeof(IEnumerable<string>), DotNetType.MakeIEnumerable(new DotNetType("System.String")) }
			};
		}


		[Theory]
		[MemberData(nameof(GetTestData))]
		public void Run(Type type, DotNetType expected) {
			var item = new DotNetType(type);
			Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(item));
		}
	}
}
