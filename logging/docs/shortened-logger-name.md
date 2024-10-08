# Shortened Logger Name
The Logger name aka the category name in Microsoft Logging aka the SourceContext in serilog are usually the full namespace of the class if the logger is injected using the `ILogger<>` interface.

```csharp
namespace MyNameSpace {
	public class MyServiceClass {
		ILogger logger;
		public MyServiceClass (ILogger<MyServiceClass> logger) {
			this.logger = logger;
		}
	}
}
```
Sometimes, the logger name is just too long.  The static method [Extensions.AddShortenLoggerName](../Albatross.Logging/Extensions.cs) can be used to remove the namespace portion of the logger name when registering services.  If the exclusive flag is false, only the logger name that starts with the namespace prefix will be shortened.  Otherwise only the logger name that does not start with the namespace prefix will be shortened.
```csharp
public static IServiceCollection AddMyServices(this IServiceCollection services) {
	services.AddShortenLoggerName(false, "MyNameSpace");
	return services;
}
```