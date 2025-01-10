using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	public class Conversation {
		public Conversation(string client, ulong id) {
			Client = client;
			Id = id;
		}
		EventEntry<CommandRequest>? request;
		EventEntry<CommandRequestAck>? requestAck;
		EventEntry<CommandRequestError>? requestError;
		EventEntry<CommandReply>? reply;
		EventEntry<CommandErrorReply>? errorReply;
		EventEntry<ClientAck>? clientAck;

		public ulong Id { get; set; }
		public string Client { get; set; }
		public string Type => this.request?.Message?.CommandName ?? string.Empty;
		public string Mode => this.request?.Message?.Mode.ToString() ?? string.Empty;

		public string RequestPayLoad => this.request?.Message?.Payload.ToUtf8String() ?? string.Empty;
		public double RequestAckDuration => (this.requestAck?.Entry.TimeStamp - this.request?.Entry.TimeStamp).GetValueOrDefault().TotalMilliseconds;
		public double RequestErrorDuration => (this.requestError?.Entry.TimeStamp - this.requestAck?.Entry.TimeStamp).GetValueOrDefault().TotalMilliseconds;
		public string RequestErrorMessage => this.requestError?.Message?.Message.ToUtf8String() ?? string.Empty;

		public string ReplyPayLoad => this.reply?.Message?.Payload.ToUtf8String() ?? string.Empty;
		public double ReplyDuration => (this.reply?.Entry.TimeStamp - this.requestAck?.Entry.TimeStamp).GetValueOrDefault().TotalMilliseconds;
		public double ReplyAckDuration => (this.clientAck?.Entry.TimeStamp - this.reply?.Entry.TimeStamp).GetValueOrDefault().TotalMilliseconds;

		public string ReplyErrorMessage => this.errorReply?.Message?.Message.ToUtf8String() ?? string.Empty;
		public double ReplyErrorDuration => (this.errorReply?.Entry.TimeStamp - this.request?.Entry.TimeStamp).GetValueOrDefault().TotalMilliseconds;
		public double ReplyErrorAckDuration => (this.clientAck?.Entry.TimeStamp - this.errorReply?.Entry.TimeStamp).GetValueOrDefault().TotalMilliseconds;
		public string? Sequence { get; set; }

		public List<EventEntry> Entries { get; } = new List<EventEntry>();
		public bool IsCompleted { get; internal set; }

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

		PrintPropertiesOption BuildPrintOptions() {
			var fields = new List<string> {
				nameof(Client),
				nameof(Type),
				nameof(Mode),
				nameof(RequestPayLoad),
			};
			if (requestError != null) {
				fields.Add(nameof(RequestErrorDuration));
				fields.Add(nameof(RequestErrorMessage));
			} else {
				fields.Add(nameof(RequestAckDuration));
			}
			if (errorReply != null) {
				fields.Add(nameof(ReplyErrorMessage));
				fields.Add(nameof(ReplyErrorDuration));
				fields.Add(nameof(ReplyErrorAckDuration));
			} else {
				fields.Add(nameof(ReplyPayLoad));
				fields.Add(nameof(ReplyDuration));
				fields.Add(nameof(ReplyAckDuration));
			};
			fields.Add(nameof(Sequence));
			return new PrintOptionBuilder<PrintPropertiesOption>().Property(fields.ToArray()).Build();
		}

		public async Task Write(TextWriter writer) {
			await writer.PrintProperties(this, this.BuildPrintOptions());
		}
	}
}