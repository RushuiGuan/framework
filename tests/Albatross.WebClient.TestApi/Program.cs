using Microsoft.AspNetCore.Hosting;

namespace Albatross.WebClient.TestApi {
    public class Program {
		public static void Main(string[] args) {
			new Albatross.Host.AspNetCore.KestrelWebHost<TestApiStartup>().Run(args);
		}
	}
}
