# Release Notes

## 7.6.0
* Create [HelpCommandHandler](./HelpCommandHandler.cs) that will display the help message of a command
* If the program is invoke without any command, it will display the help message without the error message - `Required command was not provided.`.
* If a command has its own handler, it will no longer be overwritten with the [GlobalCommandHandler](./GlobalCommandHandler.cs).  This gives developers more flexibility in create custom commands.
* A nullable `Parent` property has been added to the [VerbAttribute](./VerbAttribute.cs).  When set, the generated command will be added as a sub command of the parent verb's command.
* Add help messages to the global options.