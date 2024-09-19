using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

		public static bool TryReadLastLineFromAsciiStream(this Stream stream, [NotNullWhen(true)] out string? line, bool seekToEnd = true) {
			if (stream.CanSeek) {
				if (seekToEnd) { stream.Seek(0, SeekOrigin.End); }
				var position = stream.Position;
				if(position == 0){
					line = null;
					return false;
				}
				Stack<char> stack = new Stack<char>();
				while (position > 0) {
					position--;
					stream.Seek(position, SeekOrigin.Begin);
					var @byte = stream.ReadByte();
					if (@byte == '\n' && stack.Count > 0) {
						break;
					} else if (@byte != '\r' && @byte != '\n') {
						stack.Push((char)@byte);
					}
				}
				if (stack.Count > 0) {
					stream.Seek(position, SeekOrigin.Begin);
					line = new string(stack.ToArray());
					return true;
				}
			}
			line = null;
			return false;
		}
	}
}
