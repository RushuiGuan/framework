using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text.RegularExpressions;

namespace Albatross.Collections.Test {
	public static class My {
		public static string SortedTestFile(int start, int count, string? fileName = null) {
			var file = fileName ?? Path.GetTempFileName();
			using (var writer = new StreamWriter(file)) {
				for (int i = start; i < start + count; i++) {
					writer.WriteLine(i);
				}
			}
			return file;
		}
		public static string StringContent(this string filePath) {
			using (var reader = new StreamReader(filePath)) {
				return reader.ReadToEnd();
			}
		}
		public static int[] Array(int start, int count) {
			var result = new int[count];
			for (int i = 0; i < count; i++) {
				result[i] = start + i;
			}
			return result;
		}
	}
}