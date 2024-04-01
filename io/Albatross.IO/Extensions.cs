using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.IO {
	public static class Extensions {
		/// <summary>
		/// Check if the parent directory of the file exists.  If not, create the directory
		/// </summary>
		/// <param name="file">expected a valid file path</param>
		/// <exception cref="ArgumentException">throw the exception if the input file path is not valid</exception>
		public static void EnsureDirectory(this string file) {
			var directory = Path.GetDirectoryName(file);
			if(directory != null) {
				if (!Directory.Exists(directory)) {
					Directory.CreateDirectory(directory);
				}
			}else {
				throw new ArgumentException($"Path {file} is not valid");
			}
		}

		public static string ConvertToFilename(this string text, string filler) {
			var sb = new StringBuilder(text.Length);
			var invalidChars = Path.GetInvalidFileNameChars().ToHashSet();
			foreach(var character in filler) {
				if(invalidChars.Contains(character)) {
					throw new ArgumentException($"Filler character {character} is not a valid file name character");
				}
			}
			foreach (var character in text) {
				if (invalidChars.Contains(character)) {
					sb.Append(filler);
				} else {
					sb.Append(character);
				}
			}
			return sb.ToString();
		}	
	}
}
