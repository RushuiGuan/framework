using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestExtensiosn {
		[Fact]
		public void TestIsDerivedFrom() {
			var compilation = Symbols.Extensions.CreateCompilation(@"
				namespace Test {
					public class MyBase { }
					public class MyClass : MyBase { }
					public class YourClass : MyClass{ }
				}
			");
			var yourClass  = compilation.GetRequiredSymbol("Test.YourClass");
			var myBase = compilation.GetRequiredSymbol("Test.MyBase");
			Assert.True(yourClass.IsDerivedFrom(myBase));

			Assert.False(myBase.IsDerivedFrom(myBase));
		}
		[Fact]
		public void TestIsConstructedFrom() {
			var compilation = Symbols.Extensions.CreateCompilation(@"
				namespace Test {
					public class MyBase<T> { }
					public class MyClass : MyBase<string> { }
				}
			");
			var genericDefinition = compilation.GetRequiredSymbol("Test.MyBase`1");
			var type = genericDefinition.Construct(compilation.GetRequiredSymbol("System.String"));
			Assert.True(type.IsConstructedFrom(genericDefinition));

			type = compilation.GetRequiredSymbol("Test.MyClass");
			Assert.False(type.IsConstructedFrom(genericDefinition));
		}
	}
}
