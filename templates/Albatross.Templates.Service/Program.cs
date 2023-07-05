using Albatross.Hosting;
using System.Threading.Tasks;

namespace Albatross.Templates.Service {
	internal class Program {
		public static Task Main(string[] args) {
			return new Albatross.Hosting.Setup(args)
				.ConfigureWebHost<Startup>()
				.RunAsync();
		}
	}
}
