using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeAnalysis.MSBuild;
using Xunit;
using Microsoft.CodeAnalysis;
using FluentAssertions;

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
			Assert.True(yourClass.IsDerivedFrom(myBase));

			Assert.False(myBase.IsDerivedFrom(myBase));
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
			var type= compilation.GetRequiredSymbol("MyClass");
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
	}
}
