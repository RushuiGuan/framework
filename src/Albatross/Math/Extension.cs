using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Math {
	public static class Extension {
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
	}
}
