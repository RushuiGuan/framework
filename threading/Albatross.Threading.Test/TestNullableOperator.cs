using Xunit;

namespace Albatross.Threading.Test {
	public struct MyStruct {
		public void Call() {
			System.Console.WriteLine("MyStruct.Call");
		}
	}
	public class TestNullableOperator {
		[Fact]
		public void Test() {
			MyStruct? myStruct = null;
			myStruct?.Call();
		}
	}
}