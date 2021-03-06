﻿using Albatross.Reflection;
using System.Reflection;
using Xunit;

namespace Albatross.Test.Reflection {
	public class ExpressionTest {
		public class A {
			public string Test1 { get; }
		}

		[Fact]
		public void TestCase1() {
			PropertyInfo p = ExpressionExtension.GetPropertyInfo<A>(args => args.Test1);
			Assert.Equal(nameof(A.Test1), p.Name);
		}
	}
}
