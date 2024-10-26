using System.Text;

namespace Albatross.CodeGen.Python.Conversions {
	public static class Extensions {
		public static string GetPythonFieldName(string name) {
			var sb = new StringBuilder();
			for (int i = 0; i < name.Length; i++) {
				if (char.IsUpper(name[i]) && i > 0) {
					if (!char.IsUpper(name[i - 1]) || i != name.Length - 1 && !char.IsUpper(name[i + 1])) {
						sb.Append('_');
					}
				}
				sb.Append(char.ToLower(name[i]));
			}
			return sb.ToString();
		}
	}
}