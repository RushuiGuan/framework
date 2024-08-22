using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestClassBuilding {
		[Theory]
		[InlineData("public class Test { }", "Test")]
		public void SimpleClass(string expected, string className) {
			var node = new CodeStack()
				.Begin(new ClassDeclaration(className))
				.End().Build();
			Assert.Equal(expected, node.ToString().Replace("\r\n", " "));
		}

		const string ClassWithAttribute_Expected = @"
	[Test]
public class MyClass
{
}
";
		public void ClassWithAttribute() {
		}
	}
}
