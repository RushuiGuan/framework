﻿using Albatross.Hosting.Utility;
using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
using SampleProject.Proxy;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("pub")]
	public class PublishOption : BaseOption {
		[Option("min", Required = true)]
		public int Min { get; set; }

		[Option("max", Required = true)]
		public int Max { get; set; }

		[Option('t', "topic", Required = true)]
		public string Topic { get; set; } = string.Empty;
	}
	public class Publish : MyUtilityBase<PublishOption> {
		public Publish(PublishOption option) : base(option) {
		}
		public async Task<int> RunUtility(ICommandClient client) {
			await client.Submit(new PublishCommand(Options.Topic, Options.Min, Options.Max));
			return 0;
		}
	}

}