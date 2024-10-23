#if NET8_0_OR_GREATER
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading;
using MessagePack;
namespace Albatross.IO {
	[MessagePackObject]
	public record class FileIndexValue<KeyType> {
		public FileIndexValue(KeyType key, long position) {
			Key = key;
			Position = position;
		}
		[Key(0)]
		public KeyType Key { get; }
		[Key(1)]
		public long Position { get; }
	}
	public static class StitchExtensions {
		public static void Add<Key>(this List<FileIndexValue<Key>> list, Key key, long position) {
			list.Add(new FileIndexValue<Key>(key, position));
		}
		public readonly static MessagePackSerializerOptions IndexSerializationOptions = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);
		public readonly static MessagePackSerializerOptions RecordSerializationOptions = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.None);
		/// <summary>
		/// Provide a way to apply sorted changes to a sorted file in the most efficient way.  The method does not combine data, but rather 
		/// replaces the sorted data in the file with the changes when the keys of the changes overlaps the keys in the file.  As an example:
		/// if the file has sorted integers: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 and the changes are 3, 8.  The resulting file will be 1, 2, 3, 8, 9, 10.
		/// </summary>
		public static async Task StitchText<Key, Record>(this TextFileStitchingOptions<Key, Record> options, IEnumerable<Record> changes, ILogger logger)
			where Key : notnull, IComparable<Key> where Record : notnull {
			if (changes.Any()) {
				var indexData = new List<FileIndexValue<Key>>();
				if (options.File.Exists) {
					var firstChangeKey = options.GetKey(changes.First());
					var lastChangeKey = options.GetKey(changes.Last());
					var changesSaved = false;
					using (var readStream = new FileStream(options.File.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, options.BufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
						using (var writeStream = new FileStream(options.TempFilename, FileMode.Create, FileAccess.Write, FileShare.None, options.BufferSize, true)) {
							using var reader = new StreamReader(readStream, options.Encoding);
							using var writer = new StreamWriter(writeStream, options.Encoding) {
								AutoFlush = true
							};
							while (!reader.EndOfStream) {
								var line = await reader.ReadLineAsync();
								if (!string.IsNullOrEmpty(line)) {
									var key = options.GetKeyFromText(line);
									if (key.CompareTo(firstChangeKey) < 0) {
										indexData.Add(key, writeStream.Position);
										await writer.WriteLineAsync(line);
										continue;
									}
									if (!changesSaved) {
										foreach (var record in changes) {
											indexData.Add(options.GetKey(record), writeStream.Position);
											await writer.WriteLineAsync(options.GetText(record));
										}
										changesSaved = true;
									}
									if (key.CompareTo(lastChangeKey) > 0) {
										indexData.Add(key, writeStream.Position);
										await writer.WriteLineAsync(line);
									}
								}
							}
							if (!changesSaved) {
								foreach (var record in changes) {
									indexData.Add(options.GetKey(record), writeStream.Position);
									await writer.WriteLineAsync(options.GetText(record));
								}
							}
						}
					}
					using (var indexStream = await new FileInfo(options.IndexFilename).OpenAsyncExclusiveWriteStreamWithRetry(options.IndexBufferSize, options.IndexRetryCount, options.IndexRetryDelay, logger)) {
						await MessagePackSerializer.SerializeAsync(indexStream, indexData, IndexSerializationOptions);
						indexStream.SetLength(indexStream.Position);
						File.Move(options.TempFilename, options.File.FullName, true);
					}
				} else {
					using (var indexStream = await new FileInfo(options.IndexFilename).OpenAsyncExclusiveWriteStreamWithRetry(options.IndexBufferSize, options.IndexRetryCount, options.IndexRetryDelay, logger)) {
						using (var stream = new FileStream(options.File.FullName, FileMode.Create, FileAccess.Write, FileShare.None, options.BufferSize, FileOptions.Asynchronous)) {
							using (var writer = new StreamWriter(stream, options.Encoding) {
								AutoFlush = true,
							}) {
								foreach (var record in changes) {
									indexData.Add(options.GetKey(record), stream.Position);
									await writer.WriteLineAsync(options.GetText(record));
								}
							}
						}
						await MessagePackSerializer.SerializeAsync(indexStream, indexData, IndexSerializationOptions);
						indexStream.SetLength(indexStream.Position);
					}
				}
			}
		}
		public static async Task StitchBinary<Key, Record>(this FileStitchingOptions<Key, Record> options, IEnumerable<Record> changes, ILogger logger)
			where Key : notnull, IComparable<Key> where Record : notnull {
			if (changes.Any()) {
				var indexData = new List<FileIndexValue<Key>>();
				if (options.File.Exists) {
					var firstChangeKey = options.GetKey(changes.First());
					var lastChangeKey = options.GetKey(changes.Last());
					var changesSaved = false;
					using (var readStream = new FileStream(options.File.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, options.BufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
						using (var writeStream = new FileStream(options.TempFilename, FileMode.Create, FileAccess.Write, FileShare.None, options.BufferSize, true)) {
							var items = readStream.ReadAsStream<Record>(RecordSerializationOptions);
							await foreach (var record in items) {
								var key = options.GetKey(record);
								if (key.CompareTo(firstChangeKey) < 0) {
									indexData.Add(key, writeStream.Position);
									await MessagePackSerializer.SerializeAsync(writeStream, record, cancellationToken: CancellationToken.None);
									continue;
								}
								if (!changesSaved) {
									foreach (var change in changes) {
										indexData.Add(options.GetKey(change), writeStream.Position);
										await MessagePackSerializer.SerializeAsync(writeStream, change, cancellationToken: CancellationToken.None);
									}
									changesSaved = true;
								}
								if (key.CompareTo(lastChangeKey) > 0) {
									indexData.Add(key, writeStream.Position);
									await MessagePackSerializer.SerializeAsync(writeStream, record, cancellationToken: CancellationToken.None);
								}
							}
							if (!changesSaved) {
								foreach (var record in changes) {
									indexData.Add(options.GetKey(record), writeStream.Position);
									await MessagePackSerializer.SerializeAsync(writeStream, record, cancellationToken: CancellationToken.None);
								}
							}
							writeStream.SetLength(writeStream.Position);
						}
					}
					using (var indexStream = await new FileInfo(options.IndexFilename).OpenAsyncExclusiveWriteStreamWithRetry(options.IndexBufferSize, options.IndexRetryCount, options.IndexRetryDelay, logger)) {
						await MessagePackSerializer.SerializeAsync(indexStream, indexData, IndexSerializationOptions);
						indexStream.SetLength(indexStream.Position);
						File.Move(options.TempFilename, options.File.FullName, true);
					}
				} else {
					using (var indexStream = await new FileInfo(options.IndexFilename).OpenAsyncExclusiveWriteStreamWithRetry(options.IndexBufferSize, options.IndexRetryCount, options.IndexRetryDelay, logger)) {
						using (var stream = new FileStream(options.File.FullName, FileMode.Create, FileAccess.Write, FileShare.None, options.BufferSize, FileOptions.Asynchronous)) {
							foreach (var record in changes) {
								indexData.Add(options.GetKey(record), stream.Position);
								await MessagePackSerializer.SerializeAsync<Record>(stream, record, cancellationToken: CancellationToken.None);
							}
							stream.SetLength(stream.Position);
							await MessagePackSerializer.SerializeAsync(indexStream, indexData, IndexSerializationOptions);
							indexStream.SetLength(indexStream.Position);
						}
					}
				}
			}
		}

		public static async IAsyncEnumerable<Record> ReadAsStream<Record>(this Stream stream, MessagePackSerializerOptions options) {
			using var reader = new MessagePackStreamReader(stream);
			for (var value = await reader.ReadAsync(CancellationToken.None); value != null; value = await reader.ReadAsync(CancellationToken.None)) {
				var msg = MessagePackSerializer.Deserialize<Record>(value.Value, options);
				yield return msg;
			}
		}
		public static async Task<FileIndexValue<Key>[]> ReadFileIndex<Key>(this string indexFilename) {
			var file = new FileInfo(indexFilename);
			if (file.Exists) {
				using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)) {
					var result = await MessagePackSerializer.DeserializeAsync<FileIndexValue<Key>[]>(stream, IndexSerializationOptions);
					return result;
				}
			}else{
				return Array.Empty<FileIndexValue<Key>>();
			}
		}
	}
}
#endif
