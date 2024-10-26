using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.Messaging.EventSource {
	public class DiskStorageEventReader : IEventReader {
		private readonly DiskStorageConfiguration config;
		private readonly IMessageFactory messageFactory;
		private readonly ILogger logger;

		public DiskStorageEventReader(DiskStorageConfiguration config, IMessageFactory messageFactory, ILogger logger) {
			this.config = config;
			this.messageFactory = messageFactory;
			this.logger = logger;
		}

		public IEnumerable<EventEntry> ReadLast(TimeSpan span) {
			DateTime cutOff = DateTime.UtcNow - span;
			string filename = config.FileName.GetEventSourceFileName(cutOff);
			Stack<string> stack = new Stack<string>();
			foreach (var file in Directory.GetFiles(config.WorkingDirectory, config.FileName.GetEventSourceFilePattern()).OrderByDescending(args => args)) {
				stack.Push(file);
				if (string.Compare(Path.GetFileName(file), filename) <= 0) {
					break;
				}
			}
			while (stack.Any()) {
				var file = stack.Pop();
				using var stream = new FileInfo(file).Open(new FileStreamOptions() { Access = FileAccess.Read, Mode = FileMode.Open, Share = FileShare.ReadWrite });
				using (var reader = new StreamReader(stream)) {
					while (!reader.EndOfStream) {
						var line = reader.ReadLine();
						if (line != null) {
							EventEntry? replay = null;
							try {
								EventEntry.TryParseLine(messageFactory, line, out replay);
							} catch (Exception ex) {
								logger.LogError(ex, "Error parsing log entry: {line}", line);
							}
							if (replay?.TimeStamp >= cutOff) {
								yield return replay;
							}
						}
					}
				}
			}
		}
	}
}