## New WebApi Application
Albatross Hosting library provides a quick way of setting up a new aspnet core web application with logging, config and caching capabilities.  Besides the standard mvc and webapi controllers, it can also host SPAs (single page applications) such as SPAs created using Angular.  This article will focus on creation of a WebApi application using the Albatross.Hosting framework.  Follow the steps below using visual studio 2022 to create a new WebApi project.

1. Create a new Console App using .NET 7.0
1. Right click and edit the project file 
	* Change the Sdk attribute on the root Project element from `Microsoft.NET.Sdk` to `Microsoft.NET.Sdk.Web`
1. Optionally set the ImplicitUsings option of the project to false.  This setting is not friendly to junior developers and doesn't offer much to senior developers except code inconsistency.
1. Reference [Albatross.Hosting](https://www.nuget.org/packages/Albatross.Hosting) from nuget.org.  The current major version should be 6.
1. Create `appsettings.json` and `hostsettings.json` file at the project root and set the file property `Copy to Output Directory` = `Copy if newer`.
	* for `hostsettings.json` file, add the json below.  The port number 25000 is a number of your choice.  It is only used for local console debugging.
	```json 
	{ "urls": "http://*:25000" } 
	```
1. Create a new class below at the project root:
	```c#
	public class MyStartup : Albatross.Hosting.Startup {
       	public MyStartup(IConfiguration configuration) : base(configuration) {
       	}
   	}
	```
1. Update the program.cs file with the following code
	```c#
	public class Program {
		public static Task Main(string[] args) {
			return new Albatross.Hosting.Setup(args)
				.ConfigureWebHost<MyStartup>()
				.RunAsync();
		}
	}
	```
1. You should be able to find a file called `serilog.json` at the project root but the visual studio doesn't allow you to edit it because this file is created when referencing `Albatross.Hosting` assembly.  The file doesn't actually exist on disk.  To make it editable, create a file of the same name at the project root without using visual studio and copy its content.  Set the file property `Copy to Output Directory` = `Copy if newer`.
1. At this point, you have successfully create an asp.net application with serilog logging and swagger endpoints.  You should be able to run the application as a console app and see the web server running at port 25000.
1. A Swagger documentation page can be found at http://localhost:25000/swagger.  The default controller AppInfo can be found.  Trying adding a controller of your own and it should show up in the swagger page.