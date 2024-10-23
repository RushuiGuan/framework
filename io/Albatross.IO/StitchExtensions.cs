using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Polly.Retry;
using Polly;
using Microsoft.Extensions.Logging;
using System.Threading;
namespace Albatross.IO {
	public static class StitchExtensions {
#if NET8_0_OR_GREATER
		/// <summary>
		/// Provide a way to apply sorted changes to a sorted file in the most efficient way.  The method does not combine data, but rather 
		/// replaces the sorted data in the file with the changes when the keys of the changes overlaps the keys in the file.  As an example:
		/// if the file has sorted integers: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 and the changes are 3, 8.  The resulting file will be 1, 2, 3, 8, 9, 10.
		/// </summary>
		public static async Task Stitch<Key, Record>(this FileInfo file, List<Record> changes, FileStitchingOptions<Key, Record> options) where Key : notnull, IComparable<Key> where Record : notnull {
			if (changes.Count > 0) {
				if (file.Exists) {
					var firstChangeKey = options.GetKey(changes.First());
					var lastChangeKey = options.GetKey(changes.Last());
					var changesSaved = false;
					using (var readStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, options.BufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
						using (var writeStream = new FileStream(options.TempFile, FileMode.Create, FileAccess.Write, FileShare.None, options.BufferSize, true)) {
							using var reader = new StreamReader(readStream, options.Encoding);
							using var writer = new StreamWriter(writeStream, options.Encoding);
							while (!reader.EndOfStream) {
								var line = await reader.ReadLineAsync();
								if (!string.IsNullOrEmpty(line)) {
									var key = options.GetKeyFromText(line);
									if (key.CompareTo(firstChangeKey) < 0) {
										await writer.WriteLineAsync(line);
										continue;
									}
									if (!changesSaved) {
										foreach (var record in changes) {
											await writer.WriteLineAsync(options.GetText(record));
										}
										changesSaved = true;
									}
									if (key.CompareTo(lastChangeKey) > 0) {
										await writer.WriteLineAsync(line);
									}
								}
							}
							if (!changesSaved) {
								foreach (var record in changes) {
									await writer.WriteLineAsync(options.GetText(record));
								}
							}
						}
					}
					File.Move(options.TempFile, file.FullName, true);
				} else {
					using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write, FileShare.None, options.BufferSize, FileOptions.Asynchronous)) {
						using (var writer = new StreamWriter(stream)) {
							foreach (var record in changes) {
								await writer.WriteLineAsync(options.GetText(record));
							}
						}
					}
				}
			}
		}
#endif
	}
}
