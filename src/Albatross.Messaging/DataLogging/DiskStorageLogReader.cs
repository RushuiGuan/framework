using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.Messaging.DataLogging {
	public class DiskStorageLogReader : DiskStorageBase, IDataLogReader {
		private readonly IMessageFactory messageFactory;

		public DiskStorageLogReader(DiskStorageConfiguration config, IMessageFactory messageFactory, ILogger<DiskStorageLogWriter> logger) : base(config, logger){
			this.messageFactory = messageFactory;
		}
		
		public IEnumerable<IMessage> ReadLast(TimeSpan span) {
			DateTime cutOff = DateTime.Now - span;
			string filename = $"{config.FileName}_{cutOff.ToString(DataLog.TimeStampFormat)}.txt";
			Stack<string> stack = new Stack<string>();
			foreach(var file in Directory.GetFiles(config.WorkingDirectory, FilePattern).OrderByDescending(args=>args)) { 
				stack.Push(file);
				if (string.Compare(file, filename) <= 0) {
					break;
				}
			}
			while (stack.Any()) {
				var file = stack.Pop();
				using var stream = new FileInfo(file).Open(new FileStreamOptions() { Access = FileAccess.Read, Mode = FileMode.Open, Share = FileShare.ReadWrite });
				using (var reader =  new StreamReader(stream)) {
					while (!reader.EndOfStream) {
						var line = reader.ReadLine();
						if (line != null) {
							if (DataLog.TryParseLine(line, out var replay)) {
								if(replay.TimeStamp >= cutOff) {
									yield return this.messageFactory.Create(replay);
								}
							}
						}
					}
				}
			}
		}
	}
}
