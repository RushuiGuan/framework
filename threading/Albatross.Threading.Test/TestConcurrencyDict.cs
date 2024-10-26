using System.Collections.Concurrent;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Thrading.Test {
	public class TestConcurrencyDict {
		[Fact]
		public void CheckCreation() {
			ConcurrentDictionary<int, object> dict = new ConcurrentDictionary<int, object>();
			object myObj = new object();
			var b = dict.GetOrAdd(10, (key) => myObj);
			Assert.Same(myObj, b);

			b = dict.GetOrAdd(10, (key) => throw new System.Exception("shouldn't be here"));
			Assert.Same(myObj, b);
		}


		static object GetObject() {
			System.Threading.Thread.Sleep(5000);
			return new object();
		}

		[Fact]
		public async Task CheckParallelCreation() {
			ConcurrentDictionary<int, object> dict = new ConcurrentDictionary<int, object>();

			Task<object> task1 = Task.Run(() => dict.GetOrAdd(1, key => GetObject()));
			Task<object> task2 = Task.Run(() => dict.GetOrAdd(1, key => GetObject()));

			var obj1 = await task1;
			var obj2 = await task2;


			Assert.Same(obj1, obj2);
		}
	}
}