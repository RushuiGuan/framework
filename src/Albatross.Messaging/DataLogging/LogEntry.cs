using Albatross.Messaging.Messages;
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

		public LineType LineType { get; init; }
		public DateTime TimeStamp { get; init; }
		public IMessage Message { get; init; }

		public LogEntry(LineType lineType, DateTime timeStamp, IMessage message) {
			this.LineType = lineType;
			this.TimeStamp = timeStamp;
			this.Message = message;
		}

		public LogEntry(LineType lineType, IMessage message) : this(lineType, DateTime.Now, message) { }

		public static bool TryParseLine(IMessageFactory messageFactory, string line, [NotNullWhen(true)] out LogEntry? replay) {
			replay = null;
			LineType type;
			DateTime timeStamp;

			int offset = 0;
			if (line.TryGetText(Messages.Message.LogDelimiter, ref offset, out var text)) {
				if (text == Out) {
					type = LineType.Out;
				} else if (text == In) {
					type = LineType.In;
				}else if(text == Record) {
					type = LineType.Record;
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
			writer.Append(LineType).Space()
				.Append(DateTime.Now.ToString(LogEntry.TimeStampFormat)).Space();
			this.Message.WriteToText(writer);
			writer.WriteLine();
		}
	}
}