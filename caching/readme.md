## Caching Dos and Don'ts
1. Avoid caching if you can.  If you have to, follow these rules in the most strict mannor.

1. Cache key construction should have all properties as required or a constructor parameter.  Code snippet below is missing the Id property and can leading to incorrect cache key creation.
```c#
public class MyCacheKey
{
	public MyCacheKey(string name) {
		Name = name;
	}
	public int Id { get; set; }	// this property could be missed by developers
	public string Name { get; set; }
}
```
Use this instead:
```c#
public class MyCacheKey
{
	public MyCacheKey(int id, string name) {
		Id = id;
		Name = name;
	}
	public int Id { get; set; }
	public string Name { get; set; }
}
```
or this:
```c#
public class MyCacheKey
{
	public MyCacheKey(string name) {
		Name = name;
	}
	public required int Id { get; set; }
	public string Name { get; set; }
}
```