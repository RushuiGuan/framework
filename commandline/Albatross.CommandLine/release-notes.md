# Release Notes

## 7.6.0 - Sub Command Support + Upgrades in Different Areas
* The [VerbAttribute](./VerbAttribute.cs) can now be created without the handler type parameter.  The system will use the [DefaultCommandHandler](./DefaultCommandHandler.cs).
* New sub command support.  See [Sub Commands](../docs/sub-commands.md).
* Create [HelpCommandHandler](./HelpCommandHandler.cs) that will display the help message of a command
* If the RootCommand is invoke without any command, it will display the help message without the error message - `Required command was not provided.`.  Same behavior applies to any other parent commands.
* If a command has its own handler, it will no longer be overwritten with the [GlobalCommandHandler](./GlobalCommandHandler.cs).  This gives developers more flexibility in creating custom commands.
* Add help messages to the global options.