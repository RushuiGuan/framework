using System;
using System.IO;

namespace Albatross.Messaging.Services {
	/// <summary>
	/// durable counter will return true unique ids even in the case of process crashing.  The returned Id will always get bigger but it is not garanteed to be
	/// consecutive.  Under normal operations, the `NextId()` method will return n for the first time and n + 1 afterwards.  The number n is the interval to write to disk.
	/// If the process crashes at number `a`, the `NextId()` after restarting will be a + n.  Using this approach, the counter doesn't need to write to disk on every increment
	/// instead it writes to disk every n increments.  This is useful when the counter is used in a high throughput environment.
	/// </summary>
	public class DurableAtomicCounter : IAtomicCounter {
		string fileName;
		ulong counter;
		object sync = new object();
		public ulong Counter => counter;
		readonly uint WriteInterval;

		public DurableAtomicCounter(string directory) : this(directory, "durable-atomic-counter.txt", 10000) { }
		public DurableAtomicCounter(string directory, string name, uint writeInterval) {
			if (writeInterval == 0) {
				throw new ArgumentException("Write Interval cannot be 0");
			}
			if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
			fileName = Path.Join(directory, $"{name}");
			this.WriteInterval = writeInterval;
			this.counter = Read();
			Write(this.counter + WriteInterval);
		}

		ulong Read() {
			if (File.Exists(fileName)) {
				using (var reader = new StreamReader(fileName)) {
					var line = reader.ReadLine();
					if (ulong.TryParse(line, out var result)) {
						return result;
					}
				}
			}
			return 0;
		}
		void Write(ulong data) {
			if (data % WriteInterval == 0) {
				using (var writer = new StreamWriter(fileName)) {
					writer.WriteLine(data);
				}
			}
		}
		public ulong NextId() {
			lock (sync) {
				counter++;
				Write(counter);
				return counter;
			}
		}
	}
}