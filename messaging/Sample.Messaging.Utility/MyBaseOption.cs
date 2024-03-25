using Albatross.Hosting.Utility;
using CommandLine;

namespace Sample.Messaging.Utility {
	public class MyBaseOption : BaseOption {
		[Option('c', "count")]
		public int Count { get; set; } = 10;
	}
}
