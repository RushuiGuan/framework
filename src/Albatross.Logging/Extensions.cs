using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Logging {
	public static class Extensions {
		public static void RemoveLegacySlackSinkOptions() {
			Environment.SetEnvironmentVariable("Serilog__WriteTo__SlackSink__Args__SlackSinkOptions__WebHookUrl", null);
		}
	}
}
