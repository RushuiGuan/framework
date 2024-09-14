using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeAnalysis.MSBuild;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
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
	}
}
