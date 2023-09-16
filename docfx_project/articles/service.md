## New Service Application
The main difference between a service appication and a web api application is how the app is hosted.  Web apps are hosted by a web server such as IIS and service apps are hosted by windows services or unix systemd.  The setup of service app using the framework is similar to web api setup with a few differences.
1. If the service application doesn't have any web api controllers, creation of `hostsettings.json` file can be skipped.
    * Creation of the Startup Class at the project root can also be skipped.
1. Create the application specific Setup class at the project root:
	```c#
	public class MySetup : Setup {
		public MySetup(string[] args) : base(args) { }
		public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
			base.ConfigureServices(services, configuration);
			// register your DI services here
		}
	}
	```
1. Create the HostedService class at the project root:
	```c#
	public class MyHostedService : IHostedService {
		public MyHostedService(/* inject the service dependency here*/) {
		}

		public Task StartAsync(CancellationToken cancellationToken) {
            // add the startup code here
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) {
            // add the shutdown code here
			return Task.CompletedTask;
		}
	}
	```
1. Update the program.cs file with the following code
	```c#
	public class Program {
		public static Task Main(string[] args) {
			return new Albatross.MySetup(args)
                // uncomment the line below if you are hosting web controllers
				//.ConfigureWebHost<MyStartup>()
                .ConfigureServiceHost<MyHostedService>()
                .RunAsService()
				.RunAsync();
		}
	}
	```
1. In the `appsettings.json` file, create the `program` property.  The value of its child property `serviceManager` can be either `windows` or `systemd`.  `windows` is for windows service deployment and `systemd` is for unix systemd deployment.
	```json
	"program": {
		"app": "your application name",
		"serviceManager": "windows"
	},
	```