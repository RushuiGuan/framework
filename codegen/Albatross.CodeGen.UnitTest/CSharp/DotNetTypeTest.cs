using System;
using Albatross.CodeGen.CSharp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class ConvertTypeToDotNetTypeTest {

		public static IEnumerable<object[]> GetTestData() {
			return new List<object[]> {
				new object[]{ typeof(int), new DotNetType("System.Int32"){ IsValueType =true} },
				new object[]{ typeof(string), new DotNetType("System.String")},
				new object[]{ typeof(DateTime), new DotNetType("System.DateTime"){ IsValueType =true} },
				new object[]{ typeof(DateOnly), new DotNetType("System.DateOnly"){ IsValueType =true} },
				new object[]{ typeof(Guid), new DotNetType("System.Guid"){ IsValueType =true} },
				new object[]{ typeof(ConvertTypeToDotNetTypeTest), new DotNetType("Albatross.CodeGen.UnitTest.CSharp.ConvertTypeToDotNetTypeTest")},
				new object[]{ typeof(int[]), new DotNetType("System.Int32", true, false, new DotNetType[0]){ IsValueType =true} },
				new object[]{ typeof(IEnumerable<string>), DotNetType.MakeIEnumerable(new DotNetType("System.String")) }
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
