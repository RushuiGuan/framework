# Albatross.Text.Table
Convert collection of objects into a tabular string format with fluent interface..  Can print tabular data as a table or other formats using TextWriter.

## Features
* [StringTable](./StringTable.cs) class - A class that stores tablular data in string format.
* [TableOptions<>](./TableOptions.cs) class - An immutable class that contain configuration data that is used to convert instances of `IEnumerable<T>` into tabular text format.
* [TableOptionFactory](./TableOptionFactory.cs) - A threas safe factory class that contains the registrations of `TableOptions<>` as a dictionary of `Dictionary<TypeOfT, TableOptions<T>>`
* [TableOptionBuilder](./TableOptionBuilder.cs) - A builder class that can be used to create the `TableOptions` class using a fluient interface.

## How it works
The generic class [TableOptions\<T>](./TableOptions.cs) contains the configuration of transformation from type T to string.  The instance of [TableOptions\<T>](./TableOptions.cs) is immutable, therefore thread safe.  It can be declared manually, althrough it would be easier to create using a [TableOptionBuilder<>](./TableOptionBuilder.cs).

The instance of [TableOptions\<T>](./TableOptions.cs) class can be reused by registering it with the [TableOptionFactory](./TableOptionFactory.cs) class.  The [TableOptionFactory](./TableOptionFactory.cs) class is thread safe and it has a static instance.

There are a few built-in extensions methods that can convert `IEnumerable<T>` instances to other text formats such as csv, mark down table, [StringTable](./StringTable.cs).

## Quick Start
* To Print the data from `IEnumerable<T>` to a `TextWriter` as a table using the code below:
	```csharp
	var list = new List<T>();
	... populate the list ...
	var writer = System.Console.Out;
	
	\\ create a StringTable instance from the list
	list.StringTable()
		\\ print the table with header to TextWriter as a formatted table
		.Print(writer, true);	
	```
* Create a [TableOptionBuilder\<T>](./TableOptionBuilder.cs) instance and initialize it with class properties using reflection.
	```csharp
	// SetByReflection will initialize the TableOptionBuilder instance with the public instance properties as its columns.  It uses the default formatter from `BuilderExtensions.DefaultFormat`
	var builder = new TableOptionBuilder<T>().SetByReflection();
	```
* Further customize the builder with the desired behavior
	```csharp
	builder.Format("Price", "#,#0.00");
	builder.Exclude("Id");
	```
* Once done customization, create an instance of [TableOptions\<T>](./TableOptions.cs) to use.
	```csharp
	// Instance of TableOptions can be created directly from builder using its constructor.
	var options = new TableOptions<T>(builder);
	```
* These steps can be chained using fluent syntax
	```csharp
	var options = new TableOptionBuilder<T>()
		.SetByReflection()
		.Format("Price", "#,#0.00")
		.Exclude("Id")
		.Build();
	```
* The instance of `TableOptions<T>` can be registered globally with `TableOptionFactory`.  Note that both `TableOptions<T>` and `TableOptionFactory` are thread safe.
	```csharp
	// register the options instance directly
	var options = new TableOptionBuilder<T>()
		...Customize
		.Create();
	TableOptionFactory.Instance.Register(options);

	// options can be registered directly using a builder
	var builder = new TableOptionBuilder<T>();
	// customize the builder
	...
	TableOptionFactory.Instance.Register(builder);
	```
* The instance of `TableOptions<T>` can be retrieved from the factory by calling.  If no registration for `T` exists, an instance created by default builder will be registered and returned.
	```csharp
	var options = TableOptionFactory.Instance.Get<T>();
	```
* [TableOptions\<T>](./TableOptions.cs) class can be used to convert to other text baesd formats such as csv, [StringTable](./StringTable.cs) with ease.  Here is an example for [StringTable](./StringTable.cs) conversion.
	```csharp
	List<T> list = new List<T>();
	... populate the list ...
	var options = new TableOptionBuilder<T>()
		.SetByReflection()
		.Format("Price", "#,#0.00")
		.Exclude("Id")
		.Build();
	var table = new StringTable(options.ColumnOptions.Select(x => x.Header));
	foreach (var item in items) {
		table.Add(options.GetValue(item));
	}
	```