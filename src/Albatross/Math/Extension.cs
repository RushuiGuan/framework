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

		public static double? ToDouble(this decimal? value) {
			if (value.HasValue) {
				return Convert.ToDouble(value.Value);
			} else {
				return null;
			}
		}
	}
}
