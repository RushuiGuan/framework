using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Test.Reflection {
	public enum MyEnum {
		a,b,c, d, e, f,
	}
	public class TestEnum {
		[Fact]
		public void TestValueText() {
			object value = MyEnum.a;
			Assert.Equal("a", value.ToString());
			Assert.Equal("a", Enum.GetName(typeof(MyEnum), value));
		}
	}
}
