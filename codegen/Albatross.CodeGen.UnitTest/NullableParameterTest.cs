using System;
using System.Reflection;
using Xunit;

namespace Albatross.CodeGen.UnitTest {
	public class NullableParameterTest {
		public string? Name { get; set; }
		public string Name1 { get; set; } = null!;

		void MyMethod(string? a, Nullable<int> b) { }
		int? MyMethod2(string? a, Nullable<int> b) { return null; }
		string? MyMethod3(string? a, Nullable<int> b) { return null; }


		[Fact] public void TestNullableProperty() {
			var propertyInfo = this.GetType().GetProperty(nameof(Name));
			Assert.True(new NullabilityInfoContext().Create(propertyInfo!).WriteState == NullabilityState.Nullable);
			propertyInfo = this.GetType().GetProperty(nameof(Name1));
			Assert.False(new NullabilityInfoContext().Create(propertyInfo!).WriteState == NullabilityState.Nullable);
		}
		
		[Fact]
		public void TestNullability1() {
			var method = typeof(NullableParameterTest).GetMethod(nameof(MyMethod), BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new Exception();
			var paramA = method.GetParameters()[0];
			Assert.True(new NullabilityInfoContext().Create(paramA).ReadState is NullabilityState.Nullable);
			Assert.True(new NullabilityInfoContext().Create(paramA).WriteState is NullabilityState.Nullable);


			var paramB = method.GetParameters()[1];
			Assert.True(new NullabilityInfoContext().Create(paramA).ReadState is NullabilityState.Nullable);
			Assert.True(new NullabilityInfoContext().Create(paramA).WriteState is NullabilityState.Nullable);
		}

		[Fact]
		public void TestNullability2() {
			var method2 = typeof(NullableParameterTest).GetMethod(nameof(MyMethod2), BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new Exception();
			Assert.True(new NullabilityInfoContext().Create(method2.ReturnParameter).ReadState is NullabilityState.Nullable);
			Assert.True(new NullabilityInfoContext().Create(method2.ReturnParameter).WriteState is NullabilityState.Nullable);

			var method3 = typeof(NullableParameterTest).GetMethod(nameof(MyMethod3), BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new Exception();
			Assert.True(new NullabilityInfoContext().Create(method3.ReturnParameter).ReadState is NullabilityState.Nullable);
			Assert.True(new NullabilityInfoContext().Create(method3.ReturnParameter).WriteState is NullabilityState.Nullable);
		}
	}
}
