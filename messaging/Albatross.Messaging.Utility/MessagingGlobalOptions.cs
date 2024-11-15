using Albatross.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	public class MessagingGlobalOptions {
		[Option("a")]
		public string Application { get; set; } = string.Empty;

		[Option("f")]
		public string EventSourceFolder{get;set; } = string.Empty;
	}
}
