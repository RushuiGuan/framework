# Conventions

## Options naming convention
Option classes should be created with a postfix of `Options`.   As an example, either `BackupCommandOptions` or `BackupOptions` is ok.  When annoted with the [VerbAttribute](../Albatross.CommandLine/VerbAttribute.cs), the code generator will create the command with the name of `BackupCommand`.  Notice that it removes the `Options` postfix and append `Command` if it is not already there.  
```csharp
[Verb("backup", typeof(BackupCommandHandler))]
public class BackupCommandOptions {
	...
}
// generated command class
public sealed partial class BackupCommand : Command {
}	
```
## Command naming conflict
If both `BackupCommandOptions` and `BackupOptions` class are used, there will be a conflict for the command name `BackupCommand`.  The code generator will deal with it by postfixing with an incrementing number.  However this should be avoided especially when [command customization](./command-customization.md) is required.  Since a change in the order of the code generation could lead to change of the command name, it will then lead to customization of the wrong command.

## CommandHandler naming convention
There is no requirement on the naming of command handler as long as it doesn't lead to naming conflict.  It is ok to postfix with `CommandHandler` such as `BackupCommandHandler`.  But it is also ok to just use the name `Backup`.

## Generated Option Names
By convention, the default option name is always the lower case kebaberized property name.  In the code sample below, property `FileName` will have a default option name of `--file-name`.  This behavior is dictated by the options binding logic within the `System.CommandLine.NamingConventionBinder` assembly and cannot be changed.  However aliases can be added by the developers.  If the alias is single character, a single dash will be prefixed, otherwise a double dash.  No dash will be prefixed if it is already part of the alias.
```csharp
[Verb("backup", typeof(BackupCommandHandler))]
public class BackupCommandOptions {
	[Option("f", "file", "------file-name", Description = "The name of the file to backup")]
	public string FileName { get; set; } = string.Empty;
}
// generated command class
public sealed partial class BackupCommand : Command {
	public BackupCommand() : base("backup", null) {
		this.Option_FileName = new Option<string>("--file-name", "The name of the file to backup") {
			IsRequired = true
		};
		// single dash for single character
		Option_FileName.AddAlias("-f");
		// double dash for words
		Option_FileName.AddAlias("--file");
		// no prefix dash if already there
		Option_FileName.AddAlias("------file-name");
		this.AddOption(Option_FileName);
	}
	// Option property are generated with the prefix of Option_
	public Option<string> Option_FileName { get; }
}
```

## Generated Option Properties
Within the command class, the option properties are generated with the prefix of `Option_`.  