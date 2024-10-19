using Albatross.CommandLine;
using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	[Verb("seek", typeof(Seek))]
	public class Seekoptions {
		[Option("p")]
		public string Project { get; set; } = string.Empty;

		[Option("l")]
		public string? ProjectLocation { get; set; }

		[Option("c", "command")]
		public string? Command { get; set; }

		[Option("client")]
		public string? ClientIdentity { get; set; }

		[Option("i", "id")]
		public ulong? Id { get; set; }
	}
	public class Seek : BaseHandler<Seekoptions> {
		private readonly IMessageFactory messageFactory;

		public Seek(IMessageFactory messageFactory, IOptions<Seekoptions> options, ILogger logger) : base(options, logger) {
			this.messageFactory = messageFactory;
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			if (!options.Id.HasValue && string.IsNullOrEmpty(options.ClientIdentity) && string.IsNullOrEmpty(options.Command)) {
				throw new InvalidOperationException("At least one of the following options must be specified: id, client, command");
			}
			List<MessageGroup> result = new List<MessageGroup>();
			if (!string.IsNullOrEmpty(options.Project)) {
				string folder;
				if (string.IsNullOrEmpty(options.ProjectLocation)) {
					folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), options.Project);
				} else {
					folder = System.IO.Path.Combine(options.ProjectLocation, options.Project);
				}
				foreach (var file in System.IO.Directory.EnumerateFiles(folder, "*.log")) {
					var items = await SearchFile(file, messageFactory);
					result.AddRange(items);
				}
			}
			foreach (var group in result) {
				group.Write(Console.Out);
			}
			return 0;
		}

		void AddToResult(Dictionary<string, MessageGroup> result, EventEntry entry) {
			var key = $"{entry.Message.Route}-{entry.Message.Id}";
			if (result.TryGetValue(key, out var group)) {
				group.Entries.Add(entry);
			} else {
				group = new MessageGroup(entry.Message.Route ?? string.Empty, entry.Message.Id);
				group.Entries.Add(entry);
				result.Add(group.Key, group);
			}
		}

		async Task<IEnumerable<MessageGroup>> SearchFile(string file, IMessageFactory messageFactory) {
			var result = new Dictionary<string, MessageGroup>();
			using var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using (var reader = new StreamReader(stream)) {
				while (!reader.EndOfStream) {
					var line = await reader.ReadLineAsync();
					if (line != null) {
						if (EventEntry.TryParseLine(messageFactory, line, out var entry)) {
							if ((!options.Id.HasValue || options.Id == entry.Message.Id)
								&& (string.IsNullOrEmpty(options.ClientIdentity) || string.Equals(options.ClientIdentity, entry.Message.Route, StringComparison.InvariantCultureIgnoreCase))
								&& (string.IsNullOrEmpty(options.Command) || entry.Message is CommandMessage cmdMsg && cmdMsg.CommandType.Contains(options.Command, StringComparison.InvariantCultureIgnoreCase))
							) {
								AddToResult(result, entry);
							}
						}
					}
				}
			}
			return result.Values;
		}
	}
}
