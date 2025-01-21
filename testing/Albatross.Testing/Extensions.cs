using System;
using System.Collections.Generic;

namespace Albatross.Testing {
	public static class Extensions {
		public static IEnumerable<object[]> ConvertToTheory<T>(this IEnumerable<T> source) where T : notnull {
			foreach (var item in source) {
				yield return new object[] { item };
			}
		}

		public static DateTime ConvertToDateTime(this string text) {
			return DateTime.Parse(text, null, System.Globalization.DateTimeStyles.RoundtripKind);
		}
	}
}
