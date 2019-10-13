using System;
using System.Collections.Generic;
using System.IO;
using Albatross.CodeGen.CSharp.Model;
using Albatross.CodeGen.CSharp.Writer;
using NUnit.Framework;
using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Conversion;

namespace Albatross.CodeGen.UnitTest {
	[TestFixture(TestOf =typeof(WriteDotNetType))]
	public class WriteDotNetTypeTest {

		static IEnumerable<TestCaseData> Get() {
			return new TestCaseData[]{
				new TestCaseData(DotNetType.Boolean()){
					ExpectedResult = "System.Boolean",
				},
				new TestCaseData(DotNetType.Byte()){
					ExpectedResult = "System.Byte"
                },
				new TestCaseData(DotNetType.ByteArray()){
					ExpectedResult = "System.Byte[]"
                },
				new TestCaseData(DotNetType.Char()){
					ExpectedResult ="System.Char",
				},
				new TestCaseData(DotNetType.DateTime()){
					ExpectedResult = "System.DateTime",
				},
				new TestCaseData(DotNetType.DateTimeOffset()){
					ExpectedResult = "System.DateTimeOffset",
				},
				new TestCaseData(DotNetType.Decimal()){
					ExpectedResult = "System.Decimal",
				},
				new TestCaseData(DotNetType.Double()){
					ExpectedResult = "System.Double",
				},
				new TestCaseData(DotNetType.Guid()){
					ExpectedResult = "System.Guid",
				},
				new TestCaseData(DotNetType.IDbConnection()){
					ExpectedResult = typeof(System.Data.IDbConnection).FullName,
				},
				new TestCaseData(DotNetType.Integer()){
					ExpectedResult = "System.Int32",
				},
				new TestCaseData(DotNetType.Long()){
					ExpectedResult = "System.Int64",
				},
				new TestCaseData(DotNetType.Short()){
					ExpectedResult = "System.Int16",
				},
				new TestCaseData(DotNetType.Single()){
					ExpectedResult = "System.Single",
				},
				new TestCaseData(DotNetType.String()){
					ExpectedResult = "System.String",
				},
				new TestCaseData(DotNetType.TimeSpan()){
					ExpectedResult = "System.TimeSpan",
				},
				new TestCaseData(DotNetType.Void()){
					ExpectedResult = "void",
				},
				new TestCaseData(DotNetType.MakeIEnumerable(DotNetType.String())){ ExpectedResult="System.Collections.Generic.IEnumerable<System.String>" },
				new TestCaseData(new DotNetType("System.Int32", true, false, null)){ ExpectedResult="System.Int32[]" },
				new TestCaseData(DotNetType.MakeNullable(DotNetType.Integer())){ ExpectedResult= "System.Nullable<System.Int32>" },
			};
		}

		[TestCaseSource(nameof(Get))]
		public string Run(DotNetType dotNetType) {
			
			StringWriter writer = new StringWriter();
			writer.Run(new WriteDotNetType(), dotNetType);
			return writer.ToString();
		}
	}
}
