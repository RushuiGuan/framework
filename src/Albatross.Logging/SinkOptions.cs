using Serilog.Sinks.Slack.Models;
using System;

namespace Albatross.Logging {
	public class SinkOptions {
		public static SlackSinkOptions SlackSink => slackSink.Value;

		static readonly Lazy<SlackSinkOptions> slackSink = new Lazy<SlackSinkOptions>(() => new SlackSinkOptions {
			WebHookUrl = Environment.GetEnvironmentVariable("SlackSinkWebHookUrl"),
			BatchSizeLimit = 20,
			ShowDefaultAttachments = false,
			ShowPropertyAttachments = false,
			ShowExceptionAttachments = true,
		});
	}
}
