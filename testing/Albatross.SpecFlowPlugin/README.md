# Albatross.SpecFlowPlugin
A Specflow plugin that can be used to easily create a SpecFlow test assembly preconfigured with configuration, logging and dependency injection.

## Features
* Serilog logging provided by [Albatross.Logging](../../logging/Albatross.Logging/)
* Config setup provided by [Albatross.Config](../../config/Albatross.Config/)
* Configured with Microsoft Dependency Injection

## Quick Start
* Create a class `MySpecFlowHost` with base class [SpecFlowHost](./SpecFlowHost.cs) and attribute [SpecFlowHostAttribute](./SpecFlowHostAttribute.cs).
* Register services by overriding the `ConfigureServices` method.
	```csharp
	[SpecFlowHost]
	public class MySpecFlowHost : SpecFlowHost {
		public MySpecFlowHost(Assembly testAssembly, IObjectContainer rootBoDiContainer) : base(testAssembly, rootBoDiContainer) {
		}
		public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
	base.ConfigureServices(services, configuration);
		}
	}
	```
* That's it!  Dependencies can now be injected into the classes with the [Binding] attribute via the constructor.
* Only one SpecFlowHost class can be defined per test assembly.
* FeatureContext and ScenarioContext can be injected into the constructor as well.
* The SpecFlowHost uses the DOTNET_ENVIRONMENT variable to determine the environment.
