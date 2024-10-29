using Albatross.Testing;
using System.IO;

namespace Albatross.IO.Test {
	public static class My {
		public static string SortedTestFile(this string text, string? file = null) {
			file = file ?? Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				foreach (var item in text.IntArray()) {
					writer.WriteLine(item);
				}
			}
			return file;
		}
		public static string StringContent(this string file) {
			using (var reader = new StreamReader(file)) {
				return reader.ReadToEnd();
			}
		}
	}
}