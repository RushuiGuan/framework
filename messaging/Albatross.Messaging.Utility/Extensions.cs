using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.Messaging.Utility {
	public static class Extensions {
		public static IEnumerable<FileInfo> GetEventSourceFiles(this MessagingGlobalOptions options) {
			string folder;
			if (string.IsNullOrEmpty(options.EventSourceFolder)) {
				folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), options.Application);
			} else {
				folder = options.EventSourceFolder;
			}
			var directory = new DirectoryInfo(folder);
			if (!directory.Exists) {
				throw new DirectoryNotFoundException($"The folder {folder} does not exist");
			}
			foreach (var file in directory.GetFiles("*.log").OrderBy(x => x.Name)) {
				yield return file;
			}
		}

		public static IEnumerable<FileInfo> FindFilesToSearch(this IEnumerable<FileInfo> files, DateTime start, DateTime end) {
			FileInfo? currentFile = null;
			DateTime? fileStartInclusive = null;
			DateTime? fileEndExclusive = null;
			foreach (var file in files) {
				var timestamp = Albatross.Messaging.EventSource.Extensions.GetEventSourceFileTimeStamp(file.Name);
				if (end < timestamp) {
					yield break;
				}
				if (currentFile == null) {
					currentFile = file;
					fileStartInclusive = timestamp;
					fileEndExclusive = null;
				} else {
					fileEndExclusive = timestamp;
					if (end < fileStartInclusive || start >= fileEndExclusive) {
						continue;
					} else {
						yield return currentFile;
					}
					currentFile = file;
					fileStartInclusive = timestamp;
					fileEndExclusive = null;
				}
			}
			if (currentFile != null && end >= fileStartInclusive) {
				yield return currentFile;
			}
		}

		public static IEnumerable<Conversation> FindConversation(this FileInfo file, ulong? id, DateTime start, DateTime end, IMessageFactory messageFactory, Dictionary<ulong, Conversation> dict) {
			using var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using (var reader = new StreamReader(stream)) {
				while (!reader.EndOfStream) {
					var line = reader.ReadLine();
					if (!string.IsNullOrEmpty(line)) {
						if (EventEntry.TryParseLine(messageFactory, line, out var entry)) {
							if (dict.TryGetValue(entry.Message.Id, out var conversation)) {
								conversation.Add(entry);
								if (conversation.IsCompleted) {
									yield return conversation;
									dict.Remove(entry.Message.Id);
								}
							} else {
								if (entry.TimeStamp >= start && entry.TimeStamp <= end && (id == null || entry.Message.Id == id)) {
									conversation = new Conversation(entry.Message.Route ?? string.Empty, entry.Message.Id);
									if (conversation.IsCompleted) {
										yield return conversation;
									} else {
										dict.Add(entry.Message.Id, conversation);
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
