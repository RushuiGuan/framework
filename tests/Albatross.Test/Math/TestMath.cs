using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Test.Math {
	public class TestMath {
		[Fact]
		public void TestDecimal2DoubleConversion() {
			var value = 1844.5m;
			var result = Convert.ToDouble(value);
			Assert.Equal(1844.5, result);

			value = 1844.50000000000000000000M;
			result = Convert.ToDouble(value);
			Assert.NotEqual(1844.5, result);


			const decimal PreciseOne = 1.000000000000000000000000000000000000000000000000m;
			var trimmed = value / PreciseOne;
			Assert.Equal(1844.5m, trimmed);
			Assert.Equal(1844.5, Convert.ToDouble(trimmed));
		}
	}
}
