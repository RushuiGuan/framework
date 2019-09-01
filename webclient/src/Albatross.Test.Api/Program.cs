using Microsoft.AspNetCore.Hosting;

namespace Albatross.Test.Api {
    public class Program {
		public static void Main(string[] args) {
			new Albatross.Host.AspNetCore.KestrelWebHost<TestApiStartup>().Run(args);
		}
	}
}
