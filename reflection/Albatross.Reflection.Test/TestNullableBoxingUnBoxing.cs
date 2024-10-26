using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Reflection.Test {
	public class TestNullableBoxingUnBoxing {
		[Fact]
		public void RunTest() {
			int? a = null;
			object obj = a;
			Assert.Null(obj);

			a = 1;
			obj = a;
			Assert.Equal(typeof(int), obj.GetType());
		}
	}
}