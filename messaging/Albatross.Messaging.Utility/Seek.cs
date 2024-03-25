using Albatross.Config;
using Albatross.Hosting.Utility;
using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	public class MessageGroup {
		public MessageGroup(string client, ulong id) {
			Client = client;
			Id = id;
		}

		public ulong Id { get; set; }
		public string Client { get; set; }
		public string Key => $"{Client}-{Id}";

		public List<EventEntry> Entries { get; set; } = new List<EventEntry>();

		public void Write(TextWriter writer) {
			writer.WriteLine($"Client: {Client}, Id: {Id}");
			foreach (var entry in Entries) {
				entry.Write(writer);
				if (entry.Message is CommandRequest request) {
					writer.WriteLine(request.Payload.ToUtf8String());
				}else if(entry.Message is CommandReply reply) {
					writer.WriteLine(reply.Payload.ToUtf8String());
				}else if(entry.Message is CommandErrorReply error) {
					writer.WriteLine(error.Message.ToUtf8String());
				}else if(entry.Message is CommandRequestError requestError) {
					writer.WriteLine(requestError.Message.ToUtf8String());
				}
			}
		}
	}


	[Verb("seek")]
	public class SeekOption : BaseOption {
		[Option('f', "file", Required = true, SetName = "Search by file")]
		public string File { get; set; } = string.Empty;

		[Option('p', "project", Required = true, SetName = "Search by project")]
		public string Project { get; set; } = string.Empty;

		[Option('l', "location", SetName = "Search by project")]
		public string? ProjectLocation { get; set; }

		[Option('c', "command")]
		public string? Command { get; set; }

		[Option("client")]
		public string? ClientIdentity { get; set; }

		[Option('i', "id")]
		public ulong? Id { get; set; }
	}
	public class Seek : UtilityBase<SeekOption> {
		public Seek(SeekOption option) : base(option) { }

		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddSingleton<IMessageFactory, MessageFactory>();
		}

		public async Task<int> RunUtility(IMessageFactory messageFactory) {
			if (!Options.Id.HasValue && string.IsNullOrEmpty(Options.ClientIdentity) && string.IsNullOrEmpty(Options.Command)) {
				throw new InvalidOperationException("At least one of the following options must be specified: id, client, command");
			}
			List<MessageGroup> result = new List<MessageGroup>();
			if (!string.IsNullOrEmpty(Options.File)) {
				var items = await SearchFile(this.Options.File, messageFactory);
				result.AddRange(items);
			} else {
				string folder;
				if (string.IsNullOrEmpty(Options.ProjectLocation)) {
					folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Options.Project);
				} else {
					folder = System.IO.Path.Combine(Options.ProjectLocation, Options.Project);
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
							if((!Options.Id.HasValue || Options.Id == entry.Message.Id) 
								&& (string.IsNullOrEmpty(Options.ClientIdentity) || string.Equals(Options.ClientIdentity, entry.Message.Route, StringComparison.InvariantCultureIgnoreCase))
								&& (string.IsNullOrEmpty(Options.Command) || entry.Message is CommandMessage cmdMsg &&  cmdMsg.CommandType.Contains(Options.Command, StringComparison.InvariantCultureIgnoreCase))
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
