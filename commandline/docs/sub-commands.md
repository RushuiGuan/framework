# Sub Commands

Sub command capability is one of the differentiators between `System.CommandLine` and other parsers.  To create sub commands using `Albatross`, use a `VerbAttribute` with a space delimited name.  


```csharp
[Verb("search id")]
public class MySubOptions { 
}
```
The example above tells the system to create a command `id` with the parent command `search`.  The parent command options can be declared explicitly.  If missing, the system will create one automatically.  See more detailed examples in [Mutually Exclusive Options](./mutually-exclusive-options.md#the-new-way).

