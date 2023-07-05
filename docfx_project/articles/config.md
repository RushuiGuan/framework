## Configure the applciation
Now that we have a working aspnet application, we should work to configure it for different environments.  Albatross framework follows the concept of **single code base deployment**.  That means the deployed code should be exactly the same across different environments such as dev, staging, uat and production.  This methology simplifies the deployment process and minimize deployment errors.  It lets the host environment to have the control over what configuration should be run.

The application uses environmental variable ASPNETCORE_ENVIRONMENT to determine the current environment.  When the variable is specified, the application will find a second configuration file: `appsettings.{environment}.json` and apply its value over the original appsettings.json file.  Here is an example of the original `appsettings.json` file
```json
{
    "program": {
        "app": "My domain web app",
        "group": "My Domain"
    },
    "connectionStrings": {
        "mydb": "Server=.;Database=mydb;Trusted_Connection=true;Encrypt=false"
    },
    "endpoints": {
        "messaging-server": "tcp://localhost:31001"
    },
    "authorization": {
        "authentication": "Negotiate"
    },
    "Azure:SignalR:ConnectionString": "SECRETS ARE STORED IN ENVIRONMENTAL VARIABLES",
    "Azure:SignalR:ConnectionString:backup:secondary": "SECRETS ARE STORED IN ENVIRONMENTAL VARIABLES"
}
```
Here is the appsettings.production.json file
```json
{
    "connectionStrings": {
        "mydb": "Server=prod-db-server;Database=mydb;Trusted_Connection=true;Encrypt=false"
    },
    "endpoints": {
        "messaging-server": "tcp://prod-msg-server:31001"
    }
}
```
As you can see, the production config file is much shorter than the original since it only needs to state the difference between the two.   While the environment variable ASPNETCORE_ENVIRONMENT is typically stored on the host, other configuration valures can also be stored using environment variables.  The configuration values can also be set by command prompt.  Here is the order of precedence in terms of how the configuration values are applied.  The command prompt override has the highest precedence.
1. appsettings.json
1. appsettings.environment.json override
1. environment variable override
1. command prompt override

Environment variable override can be created using double underscore as the json property seperator.  For example. a variable name of `connectionString__mydb` will override the `mydb` connection string value in the config file.  Environment variable override are often used to store secrets since password and api keys should not be in source code.

Command prompt overrides are not used often.  They can be handy for troubleshooting and testing.  It uses colon(:) as the property seperator.  For example: 
```
my-webapi.exe endpoints:messaging-server=tcp://prod-msg-server:32001
```
will change the value of the messaging-server endpoint.  More information on how aspnetcore configuration works can be found in microsoft documentation [here](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0).