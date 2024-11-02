using Albatross.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Test.Math {
	public class TestToMaxBase {
		[Fact]
		public void Run() {
			var set = new HashSet<string>();
			long tick = DateTime.Now.Ticks;
			for (int i = 0; i < 100000; i++) {
				var text = tick.ToMaxBase();
				if (set.Contains(text)) {
					throw new Exception("Duplicate value");
				} else {
					set.Add(text);
				}
				tick++;
			}
		}

		[Theory]
		[InlineData(395916494494100, "1oQKZr3bA")]
		public void GetMaxBase(long value, string expected) {
			Assert.Equal(expected, value.ToMaxBase());
		}
	}
}