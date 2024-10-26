using Albatross.Messaging.Configurations;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.Messaging.EventSource {
	/// <summary>
	/// This class is responsible to write data logs to files.  The files are organized with UTC timestamps. The files has a configurable
	/// max size limit.  Once reached, a new file will be created.  When writing the log entry, data are bufferred line by line using
	/// a <see cref="BufferedTextWriter"/> class.
	/// </summary>
	public class DiskStorageEventWriter : IEventWriter, IDisposable {
		private readonly Encoding utf8 = new UTF8Encoding(false);
		private readonly DiskStorageConfiguration config;
		private readonly ILogger logger;
		private readonly BufferedTextWriter lineWriter;
		private StreamWriter streamWriter;
		bool disposed = false;

		public DiskStorageEventWriter(string name, DiskStorageConfiguration config, ILoggerFactory loggerFactory) {
			this.config = config;
			this.logger = loggerFactory.CreateLogger($"logwriter_{name}");
			this.lineWriter = new BufferedTextWriter(name, loggerFactory);
			if (!Directory.Exists(config.WorkingDirectory)) {
				Directory.CreateDirectory(config.WorkingDirectory);
			}
			logger.LogInformation("creating disk storage logger {name} at {path}", config.FileName, config.WorkingDirectory);
			this.streamWriter = GetWriter();
		}
		FileInfo NewFile() => new FileInfo(Path.Join(config.WorkingDirectory, config.FileName.GetEventSourceFileName()));
		StreamWriter Open(FileInfo file) {
			var stream = file.Open(new FileStreamOptions() { Access = FileAccess.ReadWrite, Mode = FileMode.OpenOrCreate, Share = FileShare.Read });
			var writer = new StreamWriter(stream, utf8);
			writer.AutoFlush = true;
			if (stream.Length > 0) {
				stream.Seek(0, SeekOrigin.End);
			}
			if (stream.Position == 0) {
				writer.WriteLine($"# maxSize {config.MaxFileSize}");
			}
			return writer;
		}

		private StreamWriter GetWriter() {
			var directory = new DirectoryInfo(config.WorkingDirectory);
			var file = directory
				.GetFiles(config.FileName.GetEventSourceFilePattern(), SearchOption.TopDirectoryOnly)
				.Where(args => args.Exists)
				.OrderByDescending(args => args.Name)
				.FirstOrDefault();
			if (file == null || file.Length > config.MaxFileSize) {
				file = NewFile();
			}
			return Open(file);
		}

		public void WriteEvent(EventEntry logEntry) {
			var writer = lineWriter.Begin();
			logEntry.Write(writer);
			this.streamWriter.Write(lineWriter.End());
			CheckSize();
		}

		void CheckSize() {
			if (streamWriter.BaseStream.Length > config.MaxFileSize) {
				streamWriter.Dispose();
				var newFile = NewFile();
				streamWriter = Open(newFile);
				logger.LogInformation("new storage created {name}", newFile.FullName);
			}
		}

		public void Dispose() {
			if (!disposed) {
				logger.LogInformation("disposing disk storage");
				this.streamWriter.Flush();
				this.streamWriter.Dispose();
				disposed = true;
				logger.LogInformation("disk storage disposed");
			}
		}
	}
}