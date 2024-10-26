using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Math {
	public static class Extensions {
		public static int GetScale(this decimal value) {
			if (value == 0) { return 0; }
			int[] bits = decimal.GetBits(value);
			return (int)((bits[3] >> 16) & 0x7F);
		}

		public static decimal? ToDecimal(this double? value) {
			if (value.HasValue) {
				return Convert.ToDecimal(value.Value);
			} else {
				return null;
			}
		}
		const decimal PreciseOne = 1.000000000000000000000000000000000000000000000000m;
		public static double ToDouble(this decimal value) {
			var converted = value / PreciseOne;
			return Convert.ToDouble(converted);
		}

		public static double? ToDouble(this decimal? value) {
			if (value.HasValue) {
				return value.Value.ToDouble();
			} else {
				return null;
			}
		}
		const string BaseDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		public static string ToMaxBase(this long value) => ToBase(value, BaseDigits.Length);
		public static string ToBase(this long value, int radix) {
			const int BitsInLong = 64;

			if (radix < 2 || radix > BaseDigits.Length) {
				throw new ArgumentException($"The radix must be betweeb 2 and {BaseDigits.Length}");
			}
			if (value == 0) { return "0"; }
			int index = BitsInLong - 1;
			long currentNumber = System.Math.Abs(value);
			char[] charArray = new char[BitsInLong];

			while (currentNumber != 0) {
				int remainder = (int)(currentNumber % radix);
				charArray[index--] = BaseDigits[remainder];
				currentNumber = currentNumber / radix;
			}

			var result = new String(charArray, index + 1, BitsInLong - index - 1);
			if (value < 0) {
				result = "-" + result;
			}
			return result;
		}


		public static string ShortUniqueString() => (DateTime.UtcNow.Ticks - DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc).Ticks).ToMaxBase();
	}
}