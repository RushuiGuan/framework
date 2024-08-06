using Albatross.Config;
using Albatross.Hosting.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Hosting.Utility {
	public class MyStartUp : StartUp{
		public MyStartUp() {
			AddCommand<HelloWorldCommand, HelloWorldCommandHandler>();
		}
	}
}
