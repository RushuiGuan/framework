using System.IO;
using System;
using System.Text;
using MessagePack;
namespace Albatross.IO {
	public class FileStitchingOptions<Key, Record> where Record : notnull where Key : notnull {
		public FileStitchingOptions(string filename, Func<Record, Key> getKey) {
			GetKey = getKey;
			this.File = new FileInfo(filename);
			this.IndexFilename = this.File.FullName + ".index";
			this.TempFilename = this.File.FullName + ".tmp";
		}

		public FileInfo File { get; }
		public Func<Record, Key> GetKey { get; }
		public int BufferSize { get; init; } = 40960;
		/// <summary>
		/// TempFile should be on the same drive as the target file to improve performance
		/// </summary>
		public string TempFilename { get; init; }

		public string IndexFilename { get; init; }
		public int IndexBufferSize { get; init; } = 4096;
		public int IndexRetryCount { get; init; } = 10;
		public int IndexRetryDelay { get; init; } = 1000;

		public MessagePackSerializerOptions IndexSerializationOptions { get; init; } = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);
		public MessagePackSerializerOptions RecordSerializationOptions { get; init; } = MessagePackSerializerOptions.Standard;
	}
	public class TextFileStitchingOptions<Key, Record> : FileStitchingOptions<Key, Record> where Record : notnull where Key : notnull {
		public TextFileStitchingOptions(string filename, Func<Record, string> getText, Func<Record, Key> getKey, Func<string, Key> getKeyFromText) : base(filename, getKey) {
			GetText = getText;
			GetKeyFromText = getKeyFromText;
		}

		public Func<Record, string> GetText { get; }
		public Func<string, Key> GetKeyFromText { get; }
		public Encoding Encoding { get; init; } = Encoding.UTF8;
	}
}
