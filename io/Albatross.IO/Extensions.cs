using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.IO {
	public static class Extensions {
		/// <summary>
		/// Check if the parent directory of the file exists.  If not, create the directory
		/// </summary>
		/// <param name="file">expected a valid file path</param>
		/// <exception cref="ArgumentException">throw the exception if the input file path is not valid</exception>
		public static void EnsureDirectory(this string file) {
			var directory = Path.GetDirectoryName(file);
			if (directory != null) {
				if (!Directory.Exists(directory)) {
					Directory.CreateDirectory(directory);
				}
			} else {
				throw new ArgumentException($"Path {file} is not valid");
			}
		}

		/// <summary>
		/// Convert the text to a valid file name by replacing invalid characters with the filler character
		/// </summary>
		/// <param name="text">the text that is used to create the file name</param>
		/// <param name="filler">the filler character used to replace the invalid characters</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">throws when the filler characters are not valid file name characters</exception>
		public static string ConvertToFilename(this string text, string filler) {
			var sb = new StringBuilder(text.Length);
			var invalidChars = Path.GetInvalidFileNameChars().ToHashSet();
			foreach (var character in filler) {
				if (invalidChars.Contains(character)) {
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

		/// <summary>
		/// prepend the content of the source stream in front of the destination stream.  The content of the source stream is not changed, however
		/// the last modified date of the source stream is updated to the current time since it is used as a buffer file.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public static async Task Prepend(this Stream destination, Stream source) {
			var srcLength = source.Length;
			source.Seek(0, SeekOrigin.End);
			await destination.CopyToAsync(source);
			destination.Seek(0, SeekOrigin.Begin);
			source.Seek(0, SeekOrigin.Begin);
			await source.CopyToAsync(destination);
			source.SetLength(srcLength);
		}
	}
}
