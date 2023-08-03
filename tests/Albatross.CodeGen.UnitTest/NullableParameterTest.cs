using System;
using System.IO;
using System.Reflection;
using Albatross.CodeGen.Core;
using Xunit;

namespace Albatross.CodeGen.UnitTest {
	public class NullableParameterTest {
		void MyMethod(string? a, Nullable<int> b) { }

		[Fact]
		public void TestNullability() {
			var method = typeof(NullableParameterTest).GetMethod(nameof(MyMethod), BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new Exception();
			var paramA = method.GetParameters()[0];
			Assert.True(new NullabilityInfoContext().Create(paramA).ReadState is NullabilityState.Nullable);
			Assert.True(new NullabilityInfoContext().Create(paramA).WriteState is NullabilityState.Nullable);


			var paramB = method.GetParameters()[1];
			Assert.True(new NullabilityInfoContext().Create(paramA).ReadState is NullabilityState.Nullable);
			Assert.True(new NullabilityInfoContext().Create(paramA).WriteState is NullabilityState.Nullable);
		}
	}
}
