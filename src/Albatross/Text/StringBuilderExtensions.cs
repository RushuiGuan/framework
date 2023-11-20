using System.Text;

namespace Albatross.Text {
	public static class StringBuilderExtensions {
		public static bool EndsWith(this StringBuilder sb, string text) {
			if (sb.Length >= text.Length) {
				for (int i = 0; i < text.Length; i++) {
					if (sb[sb.Length - text.Length + i] != text[i]) {
						return false;
					}
				}
				return true;
			}
			return false;
		}
		public static bool EndsWith(this StringBuilder sb, char c) {
			if (sb.Length >= 1) {
				return sb[sb.Length - 1] == c;
			}
			return false;
		}
	}
}
