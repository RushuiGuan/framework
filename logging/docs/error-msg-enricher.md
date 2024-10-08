# Albatross.Logging: ErrorMessageEnricher

This enricher gives logger the ability to display the Message property of the Exception on the same line as the log.  It gives the reader a quick glance of the problem and it is useful for critical applications that are monitored.  Combined with SlackSink, it could provide meaningful notifications for errors in critical applications.

To enable it, add `{ErrorMessage}` to the output template and `WithErrorMessage` to the list of enrichers.  Here is a sample serilog config file:
```json
{
	"Serilog": {
		"WriteTo": {
			"SlackSink": {
				"Name": "Slack",
				"Args": {
					"SlackSinkOptions": "Albatross.Logging.SinkOptions::SlackSink, Albatross.Logging",
					"restrictedToMinimumLevel": "Error",
					"outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ssz} {MachineName} {RequestId} {SourceContext} {ThreadId} [{Level:w3}] {Message:lj}{ErrorMessage}"
				}
			}
		},
		"Using": [
			"Albatross.Logging"
		],
		"Enrich": [
			"FromLogContext",
			"WithThreadId",
			"WithMachineName",
			"WithErrorMessage"
		]
	}
}
```
We also use custom slack sink options in this example.  See the article: [enhanced slack sink](./enhanced-slacksink.md)


