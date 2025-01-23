# 7.7.0
Change the behavior of `ConstructorDeclarationBuilder` so that it would not be defaulted to `public` access modifier.
# 7.6.2
Add `NewArrayBuilder` class
Change the behavior of `TypeNode` class.  If the type name is an empty string, the `var` type will be used.
`VariableBuilder` has a new constructor that accepts a `TypeNode` object.
# 7.6.0
Change the behavior of `InterfaceDeclarationBuilder` and `ClassDeclarationBuilder` so that they would not be defaulted to `public` access modifier.
# 7.5.9
Change the behavior of `Symbols.Extensions.GetProperties` method so that it would return the properties of the base class first.