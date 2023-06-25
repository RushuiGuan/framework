﻿using Albatross.Messaging.Messages;
using Albatross.Text;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Albatross.Messaging.DataLogging {
	public record class LogEntry {
		public const string In = "I";
		public const string Out = "O";
		public const string Record = "R";
		public const string TimeStampFormat = "yyyy-MM-ddTHH:mm:ss:fffZ";

		public EntryType EntryType { get; init; }
		public DateTime TimeStamp { get; init; }
		public IMessage Message { get; init; }

		public LogEntry(EntryType entryType, DateTime timeStamp, IMessage message) {
			this.EntryType = entryType;
			this.TimeStamp = timeStamp;
			this.Message = message;
		}

		public LogEntry(EntryType entryType, IMessage message) : this(entryType, DateTime.Now, message) { }

		public static bool TryParseLine(IMessageFactory messageFactory, string line, [NotNullWhen(true)] out LogEntry? replay) {
			replay = null;
			EntryType type;
			DateTime timeStamp;

			int offset = 0;
			if (line.TryGetText(Messages.Message.LogDelimiter, ref offset, out var text)) {
				if (text == Out) {
					type = EntryType.Out;
				} else if (text == In) {
					type = EntryType.In;
				}else if(text == Record) {
					type = EntryType.Record;
				} else {
					return false;
				}
				if (line.TryGetText(Messages.Message.LogDelimiter, ref offset, out text)) {
					if (DateTime.TryParseExact(text, LogEntry.TimeStampFormat, null, System.Globalization.DateTimeStyles.None, out timeStamp)) {
						var msg = messageFactory.Create(line, offset);
						replay = new LogEntry(type, timeStamp, msg);
						return true;
					}
				}
			}
			return false;
		}

		public void Write(TextWriter writer) {
			switch (EntryType) {
				case EntryType.Out: writer.Write(Out); break;
				case EntryType.In: writer.Write(In); break;
				case EntryType.Record: writer.Write(Record); break;
				default: writer.Write('#');break;
			}
			writer.Space()
				.Append(DateTime.Now.ToString(LogEntry.TimeStampFormat)).Space();
			this.Message.WriteToText(writer);
			writer.WriteLine();
		}
	}
}