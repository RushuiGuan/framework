using Albatross.Reflection;
using System.Reflection;
using Xunit;

namespace Albatross.Test.Reflection {
	public class ExpressionTest {
		public class A {
			public string? Test1 { get; }
			public int Number { get; }
		}

		[Fact]
		public void TestCase1() {
			PropertyInfo p = ExpressionExtensions.GetPropertyInfo<A>(args => args.Test1!);
			Assert.Equal(nameof(A.Test1), p.Name);
		}


		[Theory]
		[InlineData("test1", null, "(args.Test1 == null)")]
		[InlineData("Number", 1, "(args.Number == 1)")]
		[InlineData("test1", "a", "(args.Test1 == \"a\")")]
		public void TestCreatePredicate(string name, object value, string expected) {
			var expression = ExpressionExtensions.GetPredicate<A>(name, value);
			Assert.Equal(expected, expression.Body.ToString());
		}
	}
}
