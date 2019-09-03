using System;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using Albatross.CodeGen.TypeScript.Conversion;
using Albatross.CodeGen.TypeScript.Model;

namespace Albatross.CodeGen.UnitTest
{
    [TestFixture(TestOf = typeof(ConvertTypeToTypeScriptType))]
	public class ConvertTypeToTypeScriptTypeTest
    {

		static IEnumerable<TestCaseData> GetTestData() {
			return new TestCaseData[] {
                new TestCaseData(typeof(bool?), new TypeScriptType("boolean")),
                new TestCaseData(typeof(bool), new TypeScriptType("boolean")),
                new TestCaseData(typeof(int?), new TypeScriptType("number")),
                new TestCaseData(typeof(int), new TypeScriptType("number")),
				new TestCaseData(typeof(string), new TypeScriptType("string")),
				new TestCaseData(typeof(DateTime), new TypeScriptType("Date")),
                new TestCaseData(typeof(DateTime?), new TypeScriptType("Date")),
                new TestCaseData(typeof(Guid), new TypeScriptType("Guid")),
				new TestCaseData(typeof(ConvertTypeToTypeScriptTypeTest), new TypeScriptType("ConvertTypeToTypeScriptTypeTest")),
				new TestCaseData(typeof(int[]), new TypeScriptType("number"){ IsArray = true }),
				new TestCaseData(typeof(IEnumerable<string>), new TypeScriptType("string"){ IsArray = true, })
			};
		}



		[TestCaseSource(nameof(GetTestData))]
		public void Run(Type type, TypeScriptType expected) {
			var item = new ConvertTypeToTypeScriptType().Convert(type);
			Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(item));
		}
	}
}
