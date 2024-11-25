# Command Options
Command Options class is the entry point to create a new command.  When annotated with the [VerbAttribute](../Albatross.CommandLine/VerbAttribute.cs), code generator will pick it up and generate the appropriate command code.  The [VerbAttribute](../Albatross.CommandLine/VerbAttribute.cs) also links the command handler type with the verb using its second parameter.  Note that the command handler is associated with the verb not the options class.  Multiple command handlers can shared the same options class and multiple verbs can share the same command handler.

```csharp
[Verb("test", typeof(TestCommandHandler), Description = "test command")]
[Verb("my-command", typeof(MyCommandHandler))]
public record class TestOptions {
}
```