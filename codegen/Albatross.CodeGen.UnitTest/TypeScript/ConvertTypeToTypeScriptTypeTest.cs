using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Albatross.CodeGen.TypeScript.Conversions;
using Albatross.CodeGen.TypeScript.Models;
using Xunit;

namespace Albatross.CodeGen.UnitTest {
	public class ConvertTypeToTypeScriptTypeTest {

		public static IEnumerable<object[]> GetTestData() {
			return new List<object[]> {
				new object[]{typeof(bool?), new TypeScriptType("boolean") },
				new object[]{typeof(bool), new TypeScriptType("boolean") },
				new object[]{typeof(int?), new TypeScriptType("number") },
				new object[]{typeof(int), new TypeScriptType("number") },
				new object[]{typeof(string), new TypeScriptType("string") },
				new object[]{typeof(byte[]), new TypeScriptType("string") },
				new object[]{typeof(DateTime), new TypeScriptType("Date") },
				new object[]{typeof(DateTime?), new TypeScriptType("Date") },
				new object[]{typeof(DateOnly), new TypeScriptType("Date") },
				new object[]{typeof(DateOnly?), new TypeScriptType("Date") },
				new object[]{typeof(Guid), new TypeScriptType("Guid") },
				new object[]{typeof(ConvertTypeToTypeScriptTypeTest), new TypeScriptType("ConvertTypeToTypeScriptTypeTest") },
				new object[]{typeof(int[]), new TypeScriptType("number"){ IsArray = true } },
				new object[]{typeof(IEnumerable<string>), new TypeScriptType("string"){ IsArray = true, } }
			};
		}

		[Theory]
		[MemberData(nameof(GetTestData))]
		public void Run(Type type, TypeScriptType expected) {
			var item = new ConvertTypeToTypeScriptType().Convert(type);
			Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(item));
		}
	}
}
