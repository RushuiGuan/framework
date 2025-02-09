using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Text.Test {
	public class BehaviorTest {
		[Fact]
		public void Test() {
			var list = new List<string> { "a", "b", "c" };
			list.Insert(100, "d");
			Assert.Equal(4, list.Count);
			Assert.Equal(3, list.IndexOf("d"));
		}
	}
}
