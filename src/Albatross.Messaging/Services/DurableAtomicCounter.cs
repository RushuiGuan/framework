using System;
using System.IO;

namespace Albatross.Messaging.Services {
	/// <summary>
	/// durable counter will return true unique ids even in the case of process crashing.  The returned Id will always get bigger but it is not garanteed to be
	/// sequential.  Most of the time, the nextId() method will return the next ulong value, but in case of the process crashing, it will return the next
	/// integer number dividable by 10000.
	/// Under the hood, the class use a file to keep track of the id value.  For efficiency reason, it only updates the file when the Id % 10000 == 0.
	/// </summary>
	public class DurableAtomicCounter : IAtomicCounter<ulong> {
		string fileName;
		ulong counter;
		object sync = new object();
		public ulong Counter => counter;
		readonly uint WriteInterval;

		public DurableAtomicCounter(string directory) :this(directory, "durable-atomic-counter.txt", 10000) { }
		public DurableAtomicCounter(string directory, string name, uint writeInterval) {
			if(writeInterval == 0) {
				throw new ArgumentException("Write Interval cannot be 0");
			}
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
