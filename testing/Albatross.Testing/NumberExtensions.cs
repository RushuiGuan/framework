using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Albatross.Testing {
	public static class NumberExtensions {
		readonly static Regex Number = new Regex(@"^\s*\d+\s*$", RegexOptions.Singleline | RegexOptions.Compiled);
		readonly static Regex RangedNumber = new Regex(@"^(\d+)\s*(-[e|o]?)\s*(\d+)$", RegexOptions.Singleline | RegexOptions.Compiled);
		/// <summary>
		/// Create integer from string.  The string can be comma delimited numbers or a range of numbers separated by dash.  use -e to
		/// generate even numbers, use -o to generate odd numbers.
		/// For example, "1,2,3,5-7" will return [1,2,3,5,6,7],
		/// "1 -e 6" will generate [2,4,6] and "1 -o 6" will generate [2,4,6]
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static int[] IntArray(this string text) {
			var list = new List<int>();
			if (string.IsNullOrEmpty(text)) {
				return Array.Empty<int>();
			}
			foreach (var segment in text.Split(",")) {
				Match match = RangedNumber.Match(segment);
				if (match.Success) {
					int start = int.Parse(match.Groups[1].Value);
					int end = int.Parse(match.Groups[3].Value);
					for (int i = start; i <= end; i++) {
						list.Add(i);
					}
					if (match.Groups[2].Value == "-e") {
						list = list.Where(x => x % 2 == 0).ToList();
					} else if (match.Groups[2].Value == "-o") {
						list = list.Where(x => x % 2 == 1).ToList();
					}
				} else if (Number.IsMatch(segment)) {
					list.Add(int.Parse(segment));
				} else {
					throw new ArgumentException();
				}
			}
			return list.ToArray();
		}
		public static int[] EvenNumbers(this int[] array)
			=> array.Where(x => x % 2 == 0).ToArray();
		public static int[] OddNumbers(this int[] array)
			=> array.Where(x => x % 2 == 1).ToArray();

		public static string AsString<T>(this IEnumerable<T> array) => string.Join(",", array);
	}
}