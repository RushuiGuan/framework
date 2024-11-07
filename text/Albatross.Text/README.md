# Albatross.Text
A string manipulation library

## Features
* Print data in a tabular format
Use [PrintTextExtensions](./PrintTextExtensions.cs) static class to print collections in the tabular format.
* String interpolation
[StringInterpolationService](./StringInterpolationService.cs) provides the functionality to manipulate string by interpolation.  As shown in the example below, it seeks out expressions that are enclosed with `${` and `}` and replace it with the value provided by the input func delegate.
```csharp
[Theory]
[InlineData("abc_${b}_abc", @"{""b"":""1""}", "abc_1_abc")]
public void TestStringInterpolationWithoutObject(string text, string dictionary, string expected) {
	var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(dictionary) ?? new Dictionary<string, string>();
	var result = text.Interpolate(args => dict[args]);
	Assert.Equal(expected, result);
}
```
