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
		/// Rename the file from source to destination.  If the destination file exists, it will be overwritten
		/// For .net6.0 and above, use File.Move with overwritten = true method to perform an atomic move\delete operation.  
		/// The os will perform the operation by issuing a single command.  For netstandard2.1 code, File.Move does not have the overwrite parameter.
		/// Use File.Replace instead.  The thing about the replace command is that, if the destination file doesn't exist, it will throw an exception.  So we create
		/// a empty destination file if it doesn't exist
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		public static void MoveFileWithOverwrite(string source, string destination) {
#if NETSTANDARD2_1
			if (!File.Exists(destination)) {
				File.WriteAllText(destination, string.Empty);
			}
			File.Replace(source, destination, null, true);
#else
			File.Move(source, destination, true);
#endif
		}
	}
}