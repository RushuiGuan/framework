# .Net Hosting Environment

The .Net hosting environment variable (`DOTNET_ENVIRONMENT` or `ASPNETCORE_ENVIRONMENT`) tells the application the current environment name such as `development`, `staging` or `production`.  The value is critical since the application will load the appropriate configuration value specific to that environment. 

**When the environment variable is missing, .NET default the value to `production`.**  There are a few different reason from Microsoft: 
* Since developers should never have direct access to production, it is the safest option.  
*  Production excludes things like detailed error messages and other stuff that should never be turned on in a production environment. You must opt-in to lower security.

While this explaination are valid for contain use cases, the "security benefits" are rather small.  **The risk of updating production data by accident is very high for small to medium size enterprises where developers do have more rights than they are supposed to.**

`Albatross.Config` has a [custom implementation](../Albatross.Config/MyHostEnvironment.cs) for `IHostEnvironment` interface so that it would return "Unknown" when hosting environmental variable is not set.  The same behavior goes to the [EnvironmentSetting](../Albatross.Config/EnvironmentSetting.cs) class that serves a similar purpose.