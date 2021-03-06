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
	}
}
