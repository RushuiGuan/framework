#if NET8_0_OR_GREATER

using System.IO;
using System;
using System.Text;
namespace Albatross.IO {
	public class FileStitchingOptions<Key, Record> where Record : notnull where Key : notnull {
		public FileStitchingOptions(string indexFilename, Func<Record, string> getText, Func<Record, Key> getKey, Func<string, Key> getKeyFromText) {
			IndexFilename = indexFilename;
			GetText = getText;
			GetKey = getKey;
			GetKeyFromText = getKeyFromText;
		}

		public Func<Record, string> GetText { get; }
		public Func<Record, Key> GetKey { get; }
		public Func<string, Key> GetKeyFromText { get; }
		public Encoding Encoding { get; set; } = Encoding.UTF8;
		public int BufferSize { get; set; } = 4096;
		/// <summary>
		/// TempFile should be on the same drive as the target file to improve performance
		/// </summary>
		public string TempFile { get; set; } = Path.GetTempFileName();

		public string IndexFilename { get; }
		public int IndexBufferSize { get; set; } = 4096;
		public int IndexRetryCount { get; set; } = 10;
		public int IndexRetryDelay { get; set; } = 1000;
	}
}
#endif