using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	public class EventEntry<T> where T : IMessage {
		public EventEntry(EventEntry eventEntry) {
			Entry = eventEntry;
		}
		public EventEntry Entry { get; }
		public T Message => (T)Entry.Message;
	}

	public class MessageGroup {
		public MessageGroup(string client, ulong id) {
			Client = client;
			Id = id;
		}
		EventEntry<CommandRequest>? request;
		EventEntry<CommandReply>? reply;
		EventEntry<CommandErrorReply>? errorReply;
		EventEntry<CommandRequestError>? requestError;
		EventEntry<CommandRequestAck>? requestAck;
		EventEntry<ClientAck>? clientAck;

		public ulong Id { get; set; }
		public string Client { get; set; }
		public string Type => this.request?.Message?.CommandType ?? string.Empty;
		public string Mode => this.request?.Message?.Mode.ToString() ?? string.Empty;
		public string RequestPayLoad => this.request?.Message?.Payload.ToUtf8String() ?? string.Empty;
		public string ReplyPayLoad => this.reply?.Message?.Payload.ToUtf8String() ?? string.Empty;
		public string ErrorMessage => this.errorReply?.Message?.Message.ToUtf8String() ?? this.requestError?.Message?.Message.ToUtf8String() ?? string.Empty;
		public string Sequence { get; set; }
		public int? RequestAckDuration => (this.requestAck?.Entry.TimeStamp - this.request?.Entry.TimeStamp).GetValueOrDefault().Milliseconds;
		public int? ReplyDuration => (this.reply?.Entry.TimeStamp - this.requestAck?.Entry.TimeStamp).GetValueOrDefault().Milliseconds;
		public int? ReplyAckDuration => (this.clientAck?.Entry.TimeStamp - this.reply?.Entry.TimeStamp).GetValueOrDefault().Milliseconds;


		public List<EventEntry> Entries { get; } = new List<EventEntry>();

		public void Add(EventEntry entry) {
			if (string.IsNullOrEmpty(Sequence)) {
				Sequence = entry.Message.Header;
			} else {
				Sequence += $",{entry.Message.Header}";
			}
			this.Entries.Add(entry);
			if (entry.Message.Id == this.Id) {
				if (entry.Message is CommandRequest) {
					this.request = new EventEntry<CommandRequest>(entry);
				} else if (entry.Message is CommandReply) {
					this.reply = new EventEntry<CommandReply>(entry);
				} else if (entry.Message is CommandErrorReply) {
					this.errorReply = new EventEntry<CommandErrorReply>(entry);
				} else if (entry.Message is CommandRequestError) {
					this.requestError = new EventEntry<CommandRequestError>(entry);
				} else if (entry.Message is CommandRequestAck) {
					this.requestAck = new EventEntry<CommandRequestAck>(entry);
				} else if (entry.Message is ClientAck) {
					this.clientAck = new EventEntry<ClientAck>(entry);
				} else {
					throw new ArgumentException($"Unknown message type: {entry.Message.GetType().Name}");
				}
			} else {
				throw new ArgumentException();
			}
		}

		PrintTableOption printTableOption = new PrintOptionBuilder<PrintTableOption>()
			.Property(nameof(Id),
				nameof(Client),
				nameof(Type),
				nameof(Mode),
				nameof(RequestPayLoad),
				nameof(RequestAckDuration),
				nameof(ReplyPayLoad),
				nameof(ReplyDuration),
				nameof(ReplyAckDuration),
				nameof(ErrorMessage),
				nameof(Sequence)
			).Build();

		PrintPropertiesOption printOption = new PrintOptionBuilder<PrintPropertiesOption>()
			.Property(nameof(Id),
				nameof(Client),
				nameof(Type),
				nameof(Mode),
				nameof(RequestPayLoad),
				nameof(RequestAckDuration),

				nameof(ReplyPayLoad),
				nameof(ReplyDuration),
				nameof(ReplyAckDuration),

				nameof(ErrorMessage),
				nameof(Sequence)
			)
			.Build();

		public async Task Write(TextWriter writer) {
			await writer.PrintProperties(this, this.printOption);
		}
	}
}
