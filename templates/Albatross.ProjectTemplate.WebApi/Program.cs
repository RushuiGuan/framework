using Albatross.Hosting;

namespace Albatross.Templates.WebApi {
	internal class Program {
		public static Task Main(string[] args) {
			return new Albatross.Hosting.Setup(args)
				.ConfigureWebHost<Startup>()
				.RunAsync();
		}
	}
}
