using Albatross.Messaging.Messages;
using Albatross.Text;
using CoenM.Encoding;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.Messaging.DataLogging {
	public record class DataLog {
		public const char Space = ' ';
		public const string In = "I";
		public const string Out = "O";
		public const string Record = "R";
		public const string TimeStampFormat = "yyyyMMddHHmmssfff";

		public MessageDirection Direction { get; set; }
		public DateTime TimeStamp { get; set; }
		public string Header { get; set; }
		public string Route { get; set; }
		public ulong MessageId { get; set; }
		public List<byte[]> Payload { get; } = new List<byte[]>();

		public DataLog(string header, string route) {
			this.Header = header;
			this.Route = route;
		}

		public static bool TryParseLine(string line, [NotNullWhen(true)] out DataLog? replay) {
			replay = null;
			MessageDirection direction;
			DateTime timeStamp;
			string header;
			string route;
			ulong messageId;

			int offset = 0;
			if (line.TryGetText(Space, ref offset, out var text)) {
				if (text == Out) {
					direction = MessageDirection.Out;
				} else if (text == In) {
					direction = MessageDirection.In;
				}else if(text == Record) {
					direction = MessageDirection.None;
				} else {
					return false;
				}
				if (line.TryGetText(Space, ref offset, out text)) {
					if (text.Length == 17 && DateTime.TryParseExact(text, DataLog.TimeStampFormat, null, System.Globalization.DateTimeStyles.None, out timeStamp)) {
						if (line.TryGetText(Space, ref offset, out text)) {
							header = text;
							if (line.TryGetText(Space, ref offset, out text)) {
								route = text;
								if (line.TryGetText(Space, ref offset, out text)) {
									if (ulong.TryParse(text, out messageId)) {
										replay = new DataLog(header, route) {
											Direction = direction,
											Header = header,
											MessageId = messageId,
											TimeStamp = timeStamp,
										};

										while(line.TryGetText(Space, ref offset, out text)) {
											var bytes = Z85Extended.Decode(text);
											replay.Payload.Add(bytes);
										}
										return true;
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
	}
}