# Albatross.ReqnrollPlugin
A Reqnroll plugin that can be used to easily create a Reqnroll test assembly preconfigured with configuration, logging and dependency injection.

## Features
* Serilog logging provided by [Albatross.Logging](../../logging/Albatross.Logging/)
* Config setup provided by [Albatross.Config](../../config/Albatross.Config/)
* Configured with Microsoft Dependency Injection

## Quick Start
* Create a class `MyReqnrollHost` with base class [ReqnrollHost](./ReqnrollHost.cs) and attribute [ReqnrollHostAttribute](./ReqnrollHostAttribute.cs).
* Register services by overriding the `ConfigureServices` method.
	```csharp
	[ReqnrollHost]
	public class MyReqnrollHost : ReqnrollHost {
		public MyReqnrollHost(Assembly testAssembly, IObjectContainer rootBoDiContainer) : base(testAssembly, rootBoDiContainer) {
		}
		public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
	base.ConfigureServices(services, configuration);
		}
	}
	```
* That's it!  Dependencies can now be injected into the classes with the [Binding] attribute via the constructor.
* Only one ReqnrollHost class can be defined per test assembly.
* FeatureContext and ScenarioContext can be injected into the constructor as well.
* The ReqnrollHost uses the DOTNET_ENVIRONMENT variable to determine the environment.
