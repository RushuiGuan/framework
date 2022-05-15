using System.Text;

namespace Albatross.Text {
	public static class StringExtension {
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
	}
}
