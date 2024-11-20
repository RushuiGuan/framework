using Albatross.Messaging.Messages;
using Albatross.Text;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Albatross.Messaging.EventSource {
	public record class EventEntry {
		public const string In = "I";
		public const string Out = "O";
		public const string Record = "R";
		public const string TimeStampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

		public EntryType EntryType { get; init; }
		public DateTime TimeStamp { get; init; }
		public IMessage Message { get; init; }

		public EventEntry(EntryType entryType, DateTime timeStamp, IMessage message) {
			this.EntryType = entryType;
			this.TimeStamp = timeStamp;
			this.Message = message;
		}

		public EventEntry(EntryType entryType, IMessage message) : this(entryType, DateTime.UtcNow, message) { }

		public static bool TryParseLine(IMessageFactory messageFactory, string line, [NotNullWhen(true)] out EventEntry? replay) {
			replay = null;
			EntryType type;
			DateTime timeStamp;

			int offset = 0;
			if (line.TryGetText(Messages.Message.LogDelimiter, ref offset, out var text)) {
				if (text == Out) {
					type = EntryType.Out;
				} else if (text == In) {
					type = EntryType.In;
				} else if (text == Record) {
					type = EntryType.Record;
				} else {
					return false;
				}
				if (line.TryGetText(Messages.Message.LogDelimiter, ref offset, out text)) {
					if (DateTime.TryParseExact(text, EventEntry.TimeStampFormat, null, System.Globalization.DateTimeStyles.RoundtripKind, out timeStamp)) {
						var msg = messageFactory.Create(line, offset);
						replay = new EventEntry(type, timeStamp, msg);
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
				default: writer.Write('#'); break;
			}
			writer.Space()
				.Append(DateTime.UtcNow.ToString(EventEntry.TimeStampFormat)).Space();
			this.Message.WriteToText(writer);
			writer.WriteLine();
		}
	}
}