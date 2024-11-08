using System.IO;
using System;
using System.Text;
using MessagePack;
using System.Threading.Tasks;
namespace Albatross.IO {
	public class FileStitchingOptions<Key, Record> where Record : notnull where Key : notnull {
		public FileStitchingOptions(string filename, Func<Record, Key> getKey) {
			GetKey = getKey;
			this.DataFile = new FileInfo(filename);
			this.IndexFilename = this.DataFile.FullName + ".index";
			this.TempFilename = this.DataFile.FullName + ".tmp";
		}

		public FileInfo DataFile { get; }
		public Func<Record, Key> GetKey { get; }
		public const int DefaultBufferSize = 4096;
		/// <summary>
		/// TempFile should be on the same drive as the target file to improve performance
		/// </summary>
		public string TempFilename { get; init; }
		public string IndexFilename { get; init; }

		// by default, return a async Write stream with standard buffer size and no sharing
		public virtual Stream GetTempFileStream() {
			return new FileStream(this.TempFilename, FileMode.Create, FileAccess.Write, FileShare.None, DefaultBufferSize, true);
		}
		// default data file write stream is the same as the default temp file stream
		public virtual Stream GetDataFileWriteStream() {
			return new FileStream(DataFile.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, DefaultBufferSize, true);
		}
		// by default, return a async Read stream with 10xStandard buffer size, Read sharing and sequential scan
		public virtual Stream GetDataFileReadStream() {
			return new FileStream(DataFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize * 10, FileOptions.Asynchronous | FileOptions.SequentialScan);
		}
		// by default, return a async ReadWrite stream with standard buffer size and no sharing
		// since IndexFile is used as a locking mechanism, a retry mechanism should be in place to handle the case when someone else has the lock to the file
#if NETSTANDARD2_1
		public virtual Task<Stream> GetIndexFileStream() =>
			Task.FromResult<Stream>(new FileStream(this.IndexFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, DefaultBufferSize, true));
#else
		public virtual ValueTask<Stream> GetIndexFileStream() =>
			ValueTask.FromResult<Stream>(new FileStream(this.IndexFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, DefaultBufferSize, true));
#endif
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
