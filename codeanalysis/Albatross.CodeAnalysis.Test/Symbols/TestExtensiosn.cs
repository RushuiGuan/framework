using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Symbols {
	public class TestExtensiosn {
		[Fact]
		public void TestIsDerivedFrom() {
			var compilation = @"
				namespace Test {
					public class MyBase { }
					public class MyClass : MyBase { }
					public class YourClass : MyClass{ }
				}
			".CreateCompilation();

			var yourClass = compilation.GetRequiredSymbol("Test.YourClass");
			var myBase = compilation.GetRequiredSymbol("Test.MyBase");
			Assert.True(yourClass.IsDerivedFrom("Test.MyBase"));
			Assert.False(myBase.IsDerivedFrom("Test.MyBase"));
		}
		[Fact]
		public void TestIsConstructedFrom() {
			var compilation = @"
				namespace Test {
					public class MyBase<T> { }
					public class MyClass : MyBase<string> { }
				}
			".CreateCompilation();
			var genericDefinition = compilation.GetRequiredSymbol("Test.MyBase`1");
			var type = genericDefinition.Construct(compilation.GetRequiredSymbol("System.String"));
			Assert.True(type.IsConstructedFrom(genericDefinition));

			type = compilation.GetRequiredSymbol("Test.MyClass");
			Assert.False(type.IsConstructedFrom(genericDefinition));
		}

		[Fact]
		public void TestIsNullable() {
			var compilation = @"
using System;
public class MyClass {
	public string? Text{ get; set; }
	publis string Text2{get; set; }
	public int? Number{ get; set; }
	public Nullable<int> Number2{ get; set; }
	public int Number3{ get; set; }
}
			".CreateCompilation();
			var type = compilation.GetRequiredSymbol("MyClass");
			var textProperty = (IPropertySymbol)type.GetMembers("Text").First();
			var text2Property = (IPropertySymbol)type.GetMembers("Text2").First();

			var numberProperty = (IPropertySymbol)type.GetMembers("Number").First();
			var number2Property = (IPropertySymbol)type.GetMembers("Number2").First();
			var number3Property = (IPropertySymbol)type.GetMembers("Number3").First();

			textProperty.Type.IsNullableReferenceType().Should().BeTrue();
			textProperty.Type.IsNullableValueType().Should().BeFalse();
			text2Property.Type.IsNullableValueType().Should().BeFalse();
			text2Property.Type.IsNullableValueType().Should().BeFalse();

			numberProperty.Type.IsNullableReferenceType().Should().BeFalse();
			numberProperty.Type.IsNullableValueType().Should().BeTrue();

			number2Property.Type.IsNullableReferenceType().Should().BeFalse();
			number2Property.Type.IsNullableValueType().Should().BeTrue();

			number3Property.Type.IsNullableReferenceType().Should().BeFalse();
			number3Property.Type.IsNullableValueType().Should().BeFalse();
		}

		[Fact]
		public void TestGetNullableValueType() {
			var compilation = @"
using System;
public class MyClass {
public string? Text{ get; set; }
	public int? Number{ get; set; }
	public Nullable<int> Number2{ get; set; }
	public int Number3{ get; set; }
}
			".CreateCompilation();
			var type = compilation.GetRequiredSymbol("MyClass");
			var textProperty = (IPropertySymbol)type.GetMembers("Text").First();
			var numberProperty = (IPropertySymbol)type.GetMembers("Number").First();
			var number2Property = (IPropertySymbol)type.GetMembers("Number2").First();
			var number3Property = (IPropertySymbol)type.GetMembers("Number3").First();

			textProperty.Type.TryGetNullableValueType(out var valueType).Should().BeFalse();
			numberProperty.Type.TryGetNullableValueType(out valueType).Should().BeTrue();
			valueType!.GetFullName().Should().Be("System.Int32");

			number2Property.Type.TryGetNullableValueType(out valueType).Should().BeTrue();
			valueType!.GetFullName().Should().Be("System.Int32");

			number3Property.Type.TryGetNullableValueType(out valueType).Should().BeFalse();
		}

		[Theory]
		[InlineData("int", "System.Int32")]
		[InlineData("int?", "System.Nullable<System.Int32>")]
		[InlineData("string?", "System.String?")]
		[InlineData("string", "System.String")]
		[InlineData("int[]", "System.Int32[]")]
		[InlineData("int?[]", "System.Nullable<System.Int32>[]")]
		[InlineData("string[]", "System.String[]")]
		[InlineData("string?[]", "System.String? []")]
		public void TypeSymbol2TypeNodeConversion(string typeName, string expectedResult) {
			var code = @"class A { [Type] Field; }".Replace("[Type]", typeName);
			var compilation = code.CreateCompilation();
			var classType = compilation.GetRequiredSymbol("A");
			var type = classType.GetMembers("Field").First().As<IFieldSymbol>().Type;
			var result = type.AsTypeNode();
			result.Node.NormalizeWhitespace().ToFullString().Should().Be(expectedResult);
		}

		[Fact]
		public void TestIsOpenGenericType() {
			const string code = @"public class MyClass<T>{}";
			var compilation = code.CreateCompilation();
			var classType = compilation.GetRequiredSymbol("MyClass`1");
			classType.IsGenericTypeDefinition().Should().BeTrue();
		}

		[Theory]
		[InlineData("MyNamespace", "CommandHandler.MyClass")]
		[InlineData("MyNamespace.CommandHandler", "MyClass")]
		[InlineData("XXX", "MyNamespace.CommandHandler.MyClass")]
		public void TestGetTypeNameRelativeToNamespace(string currentNamespace, string expected) {
			const string code = @"namespace MyNamespace.CommandHandler { public class MyClass{} }";
			var compilation = code.CreateCompilation();
			var classType = compilation.GetRequiredSymbol("MyNamespace.CommandHandler.MyClass");
			classType.GetTypeNameRelativeToNamespace(currentNamespace).Should().Be(expected);
		}
	}
}