using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Symbols {
	public class TestCollectionType {
		public const string TestIsCollectionType_Code = @"
using System.Collections;
using System.Collections.Generic;
				public class MyClass {
					public List<int> List { get; set; }
					public IEnumerable<int> GenericEnumerable { get; set; }
					public IEnumerable Enumerable { get; set; }
					public ICollection<int> Collection { get; set; }
					public IList<int> IList { get; set; }
					public int[] Array { get; set; }
					public int Number { get; set; }
					public string Text { get; set; }
				}
";

		[Theory]
		[InlineData("List", true)]
		[InlineData("GenericEnumerable", true)]
		[InlineData("Enumerable", true)]
		[InlineData("Collection", true)]
		[InlineData("Array", true)]
		[InlineData("IList", true)]
		[InlineData("Number", false)]
		[InlineData("Text", false)]
		public void TestIsCollectionType(string propertyName, bool expected) {
			var compilation = TestIsCollectionType_Code.CreateCompilation();
			var type = compilation.GetRequiredSymbol("MyClass");
			var property = (IPropertySymbol)type.GetMembers(propertyName).First();
			Assert.Equal(expected, property.Type.IsCollection());
		}

		[Theory]
		[InlineData("List", true, "System.Int32")]
		[InlineData("GenericEnumerable", true, "System.Int32")]
		[InlineData("Enumerable", false, "")]
		[InlineData("Collection", true, "System.Int32")]
		[InlineData("Array", true, "System.Int32")]
		[InlineData("IList", true, "System.Int32")]
		[InlineData("Number", false, "")]
		[InlineData("Text", false, "")]
		public void TestTryGetCollectionElement(string propertyName, bool expected, string expected_element) {
			var compilation = TestIsCollectionType_Code.CreateCompilation();
			var type = compilation.GetRequiredSymbol("MyClass");
			var property = (IPropertySymbol)type.GetMembers(propertyName).First();
			var result = property.Type.TryGetCollectionElementType(out var element);
			Assert.Equal(expected, result);
			if (result) {
				Assert.Equal(expected_element, element!.GetFullName());
			}
		}
	}
}