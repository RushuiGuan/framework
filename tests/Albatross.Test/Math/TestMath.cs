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

		private readonly double[] powersOfTen_ = { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000, 10000000000 };
		private readonly int uintMaxDigits_ = (int)System.Math.Ceiling(System.Math.Log10(uint.MaxValue));

		private void HandleNonStandardPrice(decimal price, out uint integerPrice, out ushort powerOfTen) {
			var digitLength = (uint)(System.Math.Ceiling(System.Math.Log10((double)price)) + 1);
			var precision = (uint)(uintMaxDigits_ - digitLength);
			integerPrice = (uint)((double)price * powersOfTen_[precision]);
			powerOfTen = (ushort)precision;
		}

		public double ConvertPriceToDouble(uint price, uint decimalPlaces) {
			return (double)price / powersOfTen_[decimalPlaces];
		}

		[Theory]
		[InlineData("1")]
		[InlineData("1.2")]
		[InlineData("15000")]
		[InlineData("1500")]
		[InlineData("1500.0000000")]
		[InlineData("15000.0000000")]
		[InlineData("7800000")]
		[InlineData("113.609375")]
		[InlineData("1537.6")]
		public void TestHandleNonStandardPrice(string priceText) {
			decimal price = decimal.Parse(priceText);
			HandleNonStandardPrice(price, out var int_price, out var int_precision);
			var result = ConvertPriceToDouble(int_price, int_precision);
			Assert.Equal(result, Albatross.Math.Extensions.ToDouble(price));
		}
	}
}
