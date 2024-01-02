using System.Collections.Generic;
using System.IO;
using Albatross.CodeGen.CSharp.Models;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class WriteDotNetTypeTest {
		public static IEnumerable<object[]> Get() {
			return new List<object[]>{
				new object[]{DotNetType.Boolean(), "System.Boolean", },
				new object[]{DotNetType.Byte(),"System.Byte"},
				new object[]{DotNetType.ByteArray(),"System.Byte[]"},
				new object[]{DotNetType.Char(),"System.Char",  },
				new object[]{DotNetType.DateTime(),"System.DateTime",  },
				new object[]{DotNetType.DateOnly(),"System.DateOnly",  },
				new object[]{DotNetType.DateTimeOffset(),"System.DateTimeOffset",  },
				new object[]{DotNetType.Decimal(),"System.Decimal",},
				new object[]{DotNetType.Double(),"System.Double",  },
				new object[]{DotNetType.Guid(),"System.Guid",  },
				new object[]{DotNetType.IDbConnection(),typeof(System.Data.IDbConnection).FullName!,},
				new object[]{DotNetType.Integer(),"System.Int32",  },
				new object[]{DotNetType.Long(),"System.Int64", },
				new object[]{DotNetType.Short(),"System.Int16",},
				new object[]{DotNetType.Single(),"System.Single",  },
				new object[]{DotNetType.String(),"System.String",  },
				new object[]{DotNetType.TimeSpan(),"System.TimeSpan",  },
				new object[]{DotNetType.Void(),"void", },
				new object[]{DotNetType.MakeIEnumerable(DotNetType.String()),"System.Collections.Generic.IEnumerable<System.String>" },
				new object[]{new DotNetType("System.Int32", true, false, null),"System.Int32[]" },
				new object[]{DotNetType.MakeNullable(DotNetType.Integer()),"System.Nullable<System.Int32>" },
				new object[]{DotNetType.MakeNullable(DotNetType.String()),"System.String?" },
			};
		}

		[Theory]
		[MemberData(nameof(Get))]
		public void Run(DotNetType dotNetType, string expected) {
			StringWriter writer = new StringWriter();
			writer.Code(dotNetType);
			string actual = writer.ToString();
			Assert.Equal(expected, actual);
		}
	}
}
