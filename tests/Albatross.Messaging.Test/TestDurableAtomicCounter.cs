using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using Xunit;

namespace Albatross.Messaging.Test {
	public class TestDurableAtomicCounter {
		[Fact]
		public void TestStartupOperation() {
			string directory = @"c:\temp";
			string name = "my-counter.txt";
			string file = Path.Combine(directory, name);
			if (File.Exists(file)) {
				File.Delete(file);
			}
			var counter = new DurableAtomicCounter(directory, name, new Mock<ILogger>().Object);
			Assert.Equal(0UL, counter.Counter);
			Assert.Equal(1UL, counter.NextId());
		}

		[Fact]
		public void TestIncrement() {
			string directory = @"c:\temp";
			string name = "my-counter2.txt";
			string file = Path.Combine(directory, name);
			if (File.Exists(file)) {
				File.Delete(file);
			}
			var counter = new DurableAtomicCounter(directory, name, new Mock<ILogger>().Object);
			Assert.Equal(0UL, counter.Counter);
			Assert.Equal(1UL, counter.NextId());
			Assert.Equal(2UL, counter.NextId());
			counter.Dispose();
			counter = new DurableAtomicCounter(directory, name, new Mock<ILogger>().Object);
			Assert.Equal(2UL, counter.Counter);
		}

		[Fact]
		public void TestSetValue () {
			string directory = @"c:\temp";
			string name = "my-counter3.txt";
			string file = Path.Combine(directory, name);
			if (File.Exists(file)) {
				File.Delete(file);
			}
			var counter = new DurableAtomicCounter(directory, name, new Mock<ILogger>().Object);
			Assert.Equal(0UL, counter.Counter);
			counter.Set(999);
			Assert.Equal(999UL, counter.Counter);
			counter.Dispose();

			counter = new DurableAtomicCounter(directory, name, new Mock<ILogger>().Object);
			Assert.Equal(999UL, counter.Counter);
		}
	}
}
