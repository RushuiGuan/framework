using Albatross.Hosting.Utility;
using CommandLine;

namespace SampleProject.Utility {
	public class MyBaseOption : BaseOption {
		[Option('c', "count")]
		public int Count { get; set; } = 10;
	}
}
