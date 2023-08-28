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
			var counter = new DurableAtomicCounter(directory, name, 1);
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
			var counter = new DurableAtomicCounter(directory, name, 1);
			Assert.Equal(0UL, counter.Counter);
			Assert.Equal(1UL, counter.NextId());
			Assert.Equal(2UL, counter.NextId());
			counter = new DurableAtomicCounter(directory, name, 1);
			Assert.Equal(2UL, counter.Counter);
		}

		[Fact]
		public void TestWriteWithNextId() {
			string directory = @"c:\temp";
			string name = "my-counter3.txt";
			string file = Path.Combine(directory, name);
			if (File.Exists(file)) {
				File.Delete(file);
			}
			var counter = new DurableAtomicCounter(directory, name, 3);
			Assert.Equal(0UL, counter.Counter);
			Assert.Equal(1UL, counter.NextId());
			Assert.Equal(2UL, counter.NextId());
			Assert.Equal(3UL, counter.NextId());

			counter = new DurableAtomicCounter(directory, name, 1);
			Assert.Equal(3UL, counter.Counter);
			Assert.Equal(4UL, counter.NextId());
		}

		[Fact]
		public void TestTheFirstWriteAfterReadInTheConstructor() {
			string directory = @"c:\temp";
			string name = "my-counter3.txt";
			string file = Path.Combine(directory, name);
			if (File.Exists(file)) {
				File.Delete(file);
			}
			var counter = new DurableAtomicCounter(directory, name, 10);
			Assert.Equal(0UL, counter.Counter);
			Assert.Equal(1UL, counter.NextId());

			counter = new DurableAtomicCounter(directory, name, 10);
			Assert.Equal(10UL, counter.Counter);
			Assert.Equal(11UL, counter.NextId());

			counter = new DurableAtomicCounter(directory, name, 10);
			Assert.Equal(20UL, counter.Counter);
			Assert.Equal(21UL, counter.NextId());
		}

		[Fact]
		public void TestBadFileFormat() {
			string directory = @"c:\temp";
			string name = "my-counter4.txt";
			var filename = Path.Join(directory, name);
			if (File.Exists(filename)) {
				File.Delete(filename);
			}
			using(var writer = new StreamWriter(filename)) {
				writer.WriteLine("abcdefg");
				writer.WriteLine("abcdefg");
			}
			var counter = new DurableAtomicCounter(directory, name, 1);
			Assert.Equal(0UL, counter.Counter);
			Assert.Equal(1ul, counter.NextId());
		}
	}
}
