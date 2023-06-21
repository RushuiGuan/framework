using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Text;
using CoenM.Encoding;
using Microsoft.Extensions.Logging;
using NetMQ;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;

namespace Albatross.Messaging.DataLogging {
	public class DiskStorageLogWriter : DiskStorageBase, IDataLogWriter {
		StreamWriter streamWriter;
		bool disposed = false;

		public DiskStorageLogWriter(DiskStorageConfiguration config, ILogger logger) : base(config, logger){
			logger.LogInformation("creating disk storage logger {name} at {path}", config.FileName, config.WorkingDirectory);
			this.streamWriter = GetWriter();
		}
		FileInfo NewFile()=> new FileInfo(Path.Join(config.WorkingDirectory, $"{config.FileName}_{DateTime.Now.Ticks}.txt"));
		StreamWriter Open(FileInfo file) {
			var stream = file.Open(new FileStreamOptions() { Access = FileAccess.ReadWrite, Mode = FileMode.OpenOrCreate, Share = FileShare.Read });
			var writer = new StreamWriter(stream, utf8);
			writer.AutoFlush = true;
			if (stream.Length > 0) {
				stream.Seek(0, SeekOrigin.End);
			}
			if(stream.Position == 0) {
				writer.WriteLine($"# maxSize {config.MaxFileSize}");
			}
			return writer;
		}

		private StreamWriter GetWriter() {
			var directory = new DirectoryInfo(config.WorkingDirectory);
			var file = directory
				.GetFiles($"{config.FileName}_*.txt", SearchOption.TopDirectoryOnly)
				.Where(args => args.Exists)
				.OrderByDescending(args => args.Name)
				.FirstOrDefault();
			if(file == null || file.Length > config.MaxFileSize) {
				file = NewFile();
			}
			return Open(file);
		}

		public void Outgoing(IMessage message, NetMQMessage frames) {
			var writers = new TextWriter[] {
				this.streamWriter, Console.Out
			};
			foreach (var writer in writers) {
				writer.Append(DataLog.Out).Space();
				WriteTimeStamp(writer);
				writer.Append(message.Header).Space().Append(message.Route ?? string.Empty).Space().Append(message.Id);
				int index = string.IsNullOrEmpty(message.Route) ? 3 : 4;
				for (int i = index; i < frames.Count(); i++) {
					writer.Space().Append(Z85Extended.Encode(frames[i].Buffer));
				}
				writer.WriteLine();
			}
			CheckSize();
		}
		
		public void Incoming(string route, string header, ulong messageId, NetMQMessage frames) {
			var writers = new TextWriter[] {
				this.streamWriter, Console.Out
			};
			foreach (var writer in writers) {
				writer.Append(DataLog.In).Space();
				WriteTimeStamp(writer);
				writer.Append(header).Space().Append(route).Space().Append(messageId);
				foreach (var item in frames) {
					writer.Space().Append(Z85Extended.Encode(item.Buffer));
				}
				writer.WriteLine();
			}
			CheckSize();
		}
		public void Record(IMessage message) {
			var writers = new TextWriter[] {
				this.streamWriter, Console.Out
			};
			foreach (var writer in writers) {
				writer.Append(DataLog.Record).Space();
				WriteTimeStamp(writer);
				writer.Append(message.Header).Space().Append(message.Route ?? string.Empty).Space().Append(message.Id);
				writer.WriteLine();
			}
			CheckSize();
		}
		void WriteTimeStamp(TextWriter writer) {
			writer.Append(DateTime.Now.ToString(DataLog.TimeStampFormat)).Space();
		}
		void CheckSize() {
			if(streamWriter.BaseStream.Length > config.MaxFileSize) {
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
