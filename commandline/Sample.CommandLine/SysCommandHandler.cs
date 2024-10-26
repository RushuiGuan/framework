﻿using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("sys-command", typeof(SysCommandHandler), Alias = ["t"])]
	public record class SysCommandOptions {
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;

		[Option(Required = false)]
		public decimal Price { get; set; }

		[Option(Skip = true)]
		public int ShouldSkip { get; set; }

		[Option(Skip = false)]
		public int ShouldNotSkip { get; set; }

		[Option(Required = true)]
		public int? ForceRequired { get; set; }

		public ICollection<string> Items { get; set; } = new List<string>();

		[Option(Required = true)]
		public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
	}
	public class SysCommandHandler : ICommandHandler {
		private readonly ILogger<SysCommandHandler> logger;
		private readonly IConsole console;
		private readonly SysCommandOptions myOptions;

		public SysCommandHandler(ILogger<SysCommandHandler> logger, IConsole console, IOptions<SysCommandOptions> myOptions) {
			this.logger = logger;
			this.console = console;
			this.myOptions = myOptions.Value;
		}

		public int Invoke(InvocationContext context) {
			throw new NotSupportedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			logger.LogInformation("i am here");
			logger.LogInformation("my options: {myOptions}", this.myOptions);
			return Task.FromResult(0);
		}
	}
}