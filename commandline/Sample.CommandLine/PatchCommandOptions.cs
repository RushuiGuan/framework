﻿using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("patch", typeof(PatchCommandHandler), UseBaseClassProperties = false, Description = "Update an item")]
	public record class PatchCommandOptions : BaseCommandOptions {
		public string Value { get; set; } = string.Empty;
	}

	public class PatchCommandHandler : BaseHandler<PatchCommandOptions> {
		public PatchCommandHandler(IOptions<PatchCommandOptions> options, ILogger logger) : base(options, logger) {
		}

		public override Task<int> InvokeAsync(InvocationContext context) {
			return Task.FromResult(0);
		}
	}
}