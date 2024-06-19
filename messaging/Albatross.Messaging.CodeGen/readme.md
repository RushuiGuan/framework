Usage: Reference the project as an analyzer:

```xml
<PackageReference Include="Albatross.Messaging.CodeGen" Version="7.3.0" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
```

If the referenced project has a partial interface that start with I and ends with Command, the code generator will find any concrete class
that implements the inteface and add the JsonDerivedType attribute for that class to the interface. 

For example, if you have the following interface:

```csharp
public partial interface IMyCommand
{
}
```

And the following class:

```csharp
public class MyCommand : IMyCommand
{
}
```

The code generator will create a partial interface of the same name and add the following attribute to the interface:

```csharp
[JsonDerivedType(typeof(MyCommand), "MyCommand")]
public partial interface IMyCommand
{}
```

This allows the proper json serialization of the interface IMyCommand.  Note that the type discriminator is the name of the class without the namespace.
```