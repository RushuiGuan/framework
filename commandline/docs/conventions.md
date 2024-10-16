# Conventions

## Options naming convention
Option classes should be created with a postfix of `Options`.   For example, `BackupCommandOptions` or `BackupOptions`.  When annoted with the [VerbAttribute](../Albatross.CommandLine/VerbAttribute.cs), the code generator will create the command with the name of `BackupCommand`.  Notice that it removes the `Options` postfix and append `Command` if it is not already there.  Therefore both `BackupCommandOptions` class and `BackupOptions` class will lead to the same command name `BackupCommand`.  The code generator will deal with duplicate command names by Postfix them with a index.
```csharp
[Verb("backup", typeof(BackupCommandHandler))]
public class BackupCommandOptions {
}
```

