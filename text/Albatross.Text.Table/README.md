# Albatross.Text.Table
Convert collection of objects into a tabular string format with fluent interface.  Can print tabular data to console with a width limitation.  Rhe text truncate behavior of the string table can be customized for each column.

## Features
* [StringTable](./StringTable.cs) class - A class that stores tablular data in string format and provides methods to print the data to a `TextWriter` with a width limitation and truncate behavior.
* [TableOptions<>](./TableOptions.cs) class - An immutable class that contain configuration data that is used to convert instances of `IEnumerable<T>` into tabular text format.
* [TableOptionFactory](./TableOptionFactory.cs) - A threas safe factory class that contains the registrations of `TableOptions<>` as a dictionary of `Dictionary<TypeOfT, TableOptions<T>>`
* [TableOptionBuilder](./TableOptionBuilder.cs) - A builder class that can be used to create the `TableOptions` class using a fluient interface.

## How it works
The generic class [TableOptions\<T>](./TableOptions.cs) contains the configuration of transformation from type T to string.  The instance of [TableOptions\<T>](./TableOptions.cs) is immutable, therefore thread safe.  It can be declared manually, althrough it would be easier to create using a [TableOptionBuilder<>](./TableOptionBuilder.cs).

The instance of [TableOptions\<T>](./TableOptions.cs) class can be reused by registering it with the [TableOptionFactory](./TableOptionFactory.cs) class.  The [TableOptionFactory](./TableOptionFactory.cs) class is thread safe and it has a static instance.

An instance of IEnumerable<T> can be transformed into a `StringTable` instance using the created [TableOptions\<T>](./TableOptions.cs) instance.

## Printing of StringTable
* To Print the data from `IEnumerable<T>` to Console as a table using the code below:
	```csharp
	var list = new List<T>();
	... populate the list ...
	
	// create a StringTable instance from the list using the default TableOptions<T> instance
	list.StringTable().PrintConsole();	
	```
* By default, `PrintConsole` will set the total width limit of the table as the Console width.  A custom width can be set using
	```csharp
	var custom_width = 30;
	list.StringTable().AdjustColumnWidth(custom_width).Print(System.Console.Out);
	```
* The width limit of the individual columns can be configured using the fluent api.  The change below set the MinWidth of `Id` column to 10 and `Description` to 0.
	```csharp
	list.StringTable().MinWidth(x => x.Name == "Id", 10).MinWidth(x => x.Name == "Description", 0).PrintConsole();
	```
* The `StringTable.AdjustColumnWidth` method will adjust column with from right to left based on its MinWidth property first.  If the table width is still too large, the method will trim from right to left again without taking consideration of the MinWidth property.  The method will only trim what's necessary.

## Construction of TableOptions<T> Instance
`TableOptions<T>` class contains the configuration data that can convert the collection of T to string based tabular data.  It is immutable once created and it can be created using `TableOptionBuilder<T>` using its fluent api.

* Create a [TableOptionBuilder\<T>](./TableOptionBuilder.cs) instance and initialize it with class properties using reflection.
	```csharp
	// SetColumnsByReflection will initialize the TableOptionBuilder instance with the public instance properties of class T as its columns.  It uses the default formatter: `BuilderExtensions.DefaultFormat`
	var builder = new TableOptionBuilder<T>().SetColumnsByReflection();
	```
* Further customize the builder with the desired behavior
	```csharp
	// format the Price column
	builder.Format("Price", "#,#0.00");
	// Customize the data retrieve function of  the Parent column
	builder.SetColumn(x => x.Parent, x => x.Parent.Name);
	// change the header of the Parent column
	builder.ColumnHeader(x => x.Parent, "ParentName");
	// change the order of the columns
	builder.ColumnOrder(x => x.Date, 99);
	```
* Once done customization, create an instance of [TableOptions\<T>](./TableOptions.cs) to use.
	```csharp
	// Instance of TableOptions can be created directly from builder using its constructor.
	var options = new TableOptions<T>(builder);
	```
* These steps can be chained using fluent syntax
	```csharp
	var options = new TableOptionBuilder<T>()
		.SetColumnsByReflection()
		.Format("Price", "#,#0.00")
		.SetColumn(x => x.Parent, x => x.Parent.Name)
		.ColumnOrder(x => x.Date, 99)
		.Build();
	```
* The instance of `TableOptions<T>` can be registered globally with `TableOptionFactory`.  Note that both `TableOptions<T>` and `TableOptionFactory` are thread safe.
	```csharp
	// register the options instance directly
	var options = new TableOptionBuilder<T>()
		...Customize
		.Build();
	TableOptionFactory.Instance.Register(options);

	// options can be registered directly using its builder
	var builder = new TableOptionBuilder<T>();
	// customize the builder
	...
	TableOptionFactory.Instance.Register(builder);
	```
* The instance of `TableOptions<T>` can be retrieved from the factory by calling the `Get` method.  If no registration for `T` exists, an instance created by the default builder will be registered and returned.
	```csharp
	var options = TableOptionFactory.Instance.Get<T>();
	```
* [TableOptions\<T>](./TableOptions.cs) class can be used to convert collection of T to [StringTable](./StringTable.cs) with ease.
	```csharp
	var options = new TableOptionBuilder<T>()
		.SetColumnsByReflection()
		.Format("Price", "#,#0.00")
		.Exclude("Id")
		.Build();

	var list = new List<T>();
	... populate the list ...
	list.StringTable(options).PrintConsole();
	```