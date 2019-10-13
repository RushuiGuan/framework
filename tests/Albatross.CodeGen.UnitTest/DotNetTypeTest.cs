using System;
using Albatross.CodeGen.CSharp.Conversion;
using Albatross.CodeGen.CSharp.Model;
using NUnit.Framework;
using Newtonsoft;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Albatross.CodeGen.UnitTest {
	[TestFixture(TestOf = typeof(DotNetType))]
	public class ConvertTypeToDotNetTypeTest {

		static IEnumerable<TestCaseData> GetTestData() {
			return new TestCaseData[] {
				new TestCaseData(typeof(int), new DotNetType("System.Int32")),
				new TestCaseData(typeof(string), new DotNetType("System.String")),
				new TestCaseData(typeof(DateTime), new DotNetType("System.DateTime")),
				new TestCaseData(typeof(Guid), new DotNetType("System.Guid")),
				new TestCaseData(typeof(ConvertTypeToDotNetTypeTest), new DotNetType("Albatross.CodeGen.UnitTest.ConvertTypeToDotNetTypeTest")),
				new TestCaseData(typeof(int[]), new DotNetType("System.Int32", true, false, null)),
				new TestCaseData(typeof(IEnumerable<string>), DotNetType.MakeIEnumerable(new DotNetType("System.String")))
			};
		}



		[TestCaseSource(nameof(GetTestData))]
		public void Run(Type type, DotNetType expected) {
			var item = new DotNetType(type);
			Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(item));
		}
	}
}
