using System;
using System.Collections;
using System.IO;

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
	}
}
