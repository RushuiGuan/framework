# Release Notes

## 7.9.0 - Add Argument Support
## 7.8.0 - Add and implement the logic for the `OptionAttribute.DefaultToInitializer` Property
* If the `OptionAttribute.DefaultToInitializer` property is set to `true`, the code generator will generate a default value using the initializer value of the property.
## 7.7.0 - Behavior Adjustment
* If the [VerbAttribute](./VerbAttribute.cs) is created without the handler type parameter, it will default to use [HelpCommandHandler](./HelpCommandHandler.cs)instead of [DefaultCommandHandler](./DefaultCommandHandler.cs).
* References `Spectre.Console` version 0.49.1 and Add extension methods for Spectre at [SpectreExtensions](./SpectreExtensions.cs).
* Rename `VerbAttribute.Skip` property to `VerbAttribute.Ignore` property.
## 7.6.0 - Sub Command Support + Upgrades in Different Areas
* The [VerbAttribute](./VerbAttribute.cs) can now be created without the handler type parameter.  The system will use the [DefaultCommandHandler](./DefaultCommandHandler.cs).
* New sub command support.  See [Sub Commands](../docs/sub-commands.md).
* Create [HelpCommandHandler](./HelpCommandHandler.cs) that will display the help message of a command
* If the RootCommand is invoke without any command, it will display the help message without the error message - `Required command was not provided.`.  Same behavior applies to any other parent commands.
* If a command has its own handler, it will no longer be overwritten with the [GlobalCommandHandler](./GlobalCommandHandler.cs).  This gives developers more flexibility in creating custom commands.
* Add help messages to the global options.