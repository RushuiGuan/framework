using System.Threading.Tasks;

namespace Albatross.Hosting.Test{
	public class Program {
		public static Task Main(string[] args) {
			return new Setup(args).ConfigureWebHost<Startup>().ConfigureServiceHost<MyServiceHost>().RunAsync();
        }
	}
}