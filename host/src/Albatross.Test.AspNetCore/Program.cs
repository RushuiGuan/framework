using Microsoft.AspNetCore.Hosting;

namespace Albatross.Test.AspNetCore {
    public class Program {
		public static void Main(string[] args) {
            //new Albatross.Composition.AspNetCore.DefaultIISWebHost().Create<Startup>(args).Run();
            new Albatross.Host.AspNetCore.HttpSysWebHost().Create<Startup>(args).Run();
        }
	}
}