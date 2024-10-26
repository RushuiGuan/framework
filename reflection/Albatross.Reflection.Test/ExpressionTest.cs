using Albatross.Reflection;
using System.Reflection;
using Xunit;

[assembly: DefaultNamespace("Albatross.Reflection.Test")]

namespace Albatross.Reflection.Test {
	public class ExpressionTest {
		public class A {
			public string? Test1 { get; set; }
			public int Number { get; set; }
			public int? NullableNumber { get; set; }
		}

		public class B {
			public A A { get; set; }
			public B(A a) {
				A = a;
			}
		}

		[Fact]
		public void TestCase1() {
			PropertyInfo p = ExpressionExtensions.GetPropertyInfo<A>(args => args.Test1!);
			Assert.Equal(nameof(A.Test1), p.Name);
		}

		[Fact]
		public void TestCase2() {
			PropertyInfo p = ExpressionExtensions.GetPropertyInfo<B>(args => args.A.Test1!);
			Assert.Equal(nameof(A.Test1), p.Name);
		}

		[Theory]
		[InlineData("test1", null, "(args.Test1 == null)")]
		[InlineData("Number", 1, "(args.Number == 1)")]
		[InlineData("test1", "a", "(args.Test1 == \"a\")")]
		public void TestCreatePredicate(string name, object? value, string expected) {
			var expression = ExpressionExtensions.GetPredicate<A>(name, value);
			Assert.Equal(expected, expression.Body.ToString());
		}

		[Fact]
		public void TestSetValueIfNotNull() {
			var a = new A();
			a.SetValueIfNotNull(args => args.Number, null);
			Assert.Equal(0, a.Number);

			a.SetValueIfNotNull(args => args.Number, 1);
			Assert.Equal(1, a.Number);

			a.SetValueIfNotNull(args => args.Number, null);
			Assert.Equal(1, a.Number);


			a.SetValueIfNotNull(args => args.NullableNumber, null);
			Assert.Null(a.NullableNumber);

			a.SetValueIfNotNull(args => args.NullableNumber, 1);
			Assert.Equal(1, a.NullableNumber);

			a.SetValueIfNotNull(args => args.NullableNumber, null);
			Assert.Equal(1, a.NullableNumber);
		}

		[Fact]
		public void TestSetTextIfNotNull() {
			var a = new A();
			a.SetTextIfNotEmpty(args => args.Test1, null);
			Assert.True(string.IsNullOrEmpty(a.Test1));

			a.SetTextIfNotEmpty(args => args.Test1, "abc");
			Assert.Equal("abc", a.Test1);

			a.SetTextIfNotEmpty(args => args.Test1, "");
			Assert.Equal("abc", a.Test1);
		}
	}
}