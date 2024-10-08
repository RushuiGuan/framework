# Albatross.Logging: Enhanced SlackSink

Here is the default structure of the slack sink options:
```json
"SlackSink": {
	"Name": "Slack",
	"Args": {
		"SlackSinkOptions": {
			"WebHookUrl": "SECRETS ARE STORED IN SERVER ENVIRONMENT VARIABLES",
			"BatchSizeLimit": 20,
			"ShowDefaultAttachments": false,
			"ShowPropertyAttachments": false,
			"ShowExceptionAttachments": false
		},
		"restrictedToMinimumLevel": "Error",
		"outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ssz} {MachineName} {SourceContext} {ThreadId} [{Level:w3}] {Message:lj}"
	}
}
```
The WebHookUrl is a secret and has to be stored as an environment variable.  Because the environment variable is global, any application with the serilog setup and without the slack sink will fail to initialize because its SlackSinkOptions are only partially set.  Three ways out of this:
1. only run a single app per server - not always feasible
1. add this line `Extensions.RemoveLegacySlackSinkOptions()` in front of any app that don't use slack sink.  It essentially removes the environmental variable: `Serilog__WriteTo__SlackSink__Args__SlackSinkOptions__WebHookUrl` for the app.
1. Put the slack webhook in environment variable `SlackSinkWebHookUrl` and setup slack options using [SinkOptions.SlackSink](../Albatross.Logging/SinkOptions.cs).  Reference the example below.
```json
"SlackSink": {
	"Name": "Slack",
	"Args": {
		"SlackSinkOptions": "Albatross.Logging.SinkOptions::SlackSink, Albatross.Logging",
		"restrictedToMinimumLevel": "Error",
		"outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ssz} {MachineName} {SourceContext} {ThreadId} [{Level:w3}] {Message:lj}{ErrorMessage}"
	}
}
```
1. Sometimes a mix of options 2 and 3 are used because the same servers can have legacy applications that use the original `SlackSinkOptions`.