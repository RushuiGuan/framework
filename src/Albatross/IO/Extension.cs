using System;
using System.Collections;
using System.IO;

namespace Albatross.IO {
	public static class Extension {
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
