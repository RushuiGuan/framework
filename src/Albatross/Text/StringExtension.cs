using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Albatross.Text {
	public static class StringExtensions {
		public static string ProperCase(this string text) {
			if (!string.IsNullOrEmpty(text)) {
				string result = text.Substring(0, 1).ToUpper();
				if (text.Length > 1) {
					result = result + text.Substring(1);
				}
				return result;
			} else {
				return text;
			}
		}

		// CUSIP = cusip
		// BBYellow = bbYellow
		// Test = test
		// test = test
		public static string CamelCase(this string text) {
			if (!string.IsNullOrEmpty(text)) {
				if (char.IsLower(text[0])) {
					return text;
				} else {
					int marker = 0;
					StringBuilder sb = new StringBuilder(text);
					for (int i = 0; i < sb.Length; i++) {
						char c = sb[i];
						if (char.IsUpper(c)) {
							if (i == 0 || marker == i && (i == sb.Length - 1 || char.IsUpper(sb[i + 1]))) {
								sb[i] = char.ToLower(c);
								marker++;
							}
						}
					}
					return sb.ToString();
				}
			} else {
				return text;
			}
		}

		/// <summary>
		/// match a string against a glob pattern.  ? matches any single character and * matches any characters
		/// If dealing with file systems directly, please use Microsoft.Extensions.FileSystemGlobbing instead.
		/// The method will return false for null or empty text string.  It will throw an ArgumentException if the 
		/// parameter <paramref name="globPattern"/> is null.
		/// </summary>
		/// <param name="text">The string to be tested</param>
		/// <param name="globPattern">A glob pattern where ? matches any single character and * matches any characters</param>
		/// <returns></returns>
		public static bool Like(this string? text, string globPattern) {
			if (string.IsNullOrEmpty(text)) { return false; }
			if (globPattern == null) { throw new ArgumentException($"{nameof(globPattern)} cannot be null"); }
			string pattern = $"^{Regex.Escape(globPattern).Replace(@"\*", ".*").Replace(@"\?", ".")}$";
			var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			return regex.IsMatch(text);
		}

		/// <summary>
		/// Postfix the specified character to the text if the text is not empty and does not end with the said character
		/// </summary>
		/// <param name="text"></param>
		/// <param name="character"></param>
		/// <returns></returns>
		public static string? PostfixIfNotNullOrEmpty(this string? text, char character) {
			if(!string.IsNullOrEmpty(text) && !text.EndsWith(character)) {
				return text + character;
			} else {
				return text;
			}
		}
	}
}
