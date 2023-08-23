using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Albatross.Messaging.Services {
	/// <summary>
	/// durable counter will return true unique ids even in the case of process crashing.  The returned Id will always get bigger but it is not garanteed to be
	/// sequential.  Most of the time, the nextId() method will return the next ulong value, but in case of the process crashing, it will return the next
	/// integer number dividable by 10000.
	/// Under the hood, the class use a file to keep track of the id value.  For efficiency reason, it only updates the file when the Id % 10000 == 0.
	/// 
	/// 
	/// 
	/// TODO: NEED TESTING.  NEXT ID IS RETURNING THE WRONG ID
	/// </summary>
	public class DurableAtomicCounter : IAtomicCounter<ulong>, IDisposable{
		private readonly ILogger logger;
		Stream stream;
		string fileName;
		ulong counter;
		bool disposed = false;
		object sync = new object();
		public ulong Counter => counter;
		readonly uint WriteInterval;

		public DurableAtomicCounter(string directory, ILogger logger) :this(directory, "durable-atomic-counter.bin", 10000, logger) { }
		public DurableAtomicCounter(string directory, string name, uint writeInterval, ILogger logger) {
			if(writeInterval == 0) {
				throw new ArgumentException("Write Interval cannot be 0");
			}
			fileName = Path.Join(directory, $"{name}");
			this.WriteInterval = writeInterval;
			logger.LogInformation("Start DurableAtomicCounter using file {name} and write interval of {interval}", fileName, writeInterval);
			this.stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
			this.counter = Read();
			Write(this.counter + WriteInterval);
			this.logger = logger;
		}

		ulong Read() {
			if (stream.Length == 0) {
				return 0;
			} else {
				try {
					byte[] buffer = new byte[sizeof(ulong)];
					stream.ReadExactly(buffer, 0, sizeof(ulong));
					var value = BitConverter.ToUInt64(buffer, 0);
					if (value % WriteInterval != 0) {
						value = (value / WriteInterval + 1) * WriteInterval;
					}
					return value;
				} catch (Exception err) {
					logger.LogError(err, "DurableAtomicCounter: eror reading from {name}, resetting to 0", this.fileName);
					return 0;
				}
			}
		}
		void Write(ulong data) {
			if (data % WriteInterval == 0) {
				stream.Seek(0, SeekOrigin.Begin);
				var bytes = BitConverter.GetBytes(data);
				stream.Write(bytes, 0, bytes.Length);
				stream.Flush();
			}
		}

		//TODO: THIS METHOD IS WRONG.  IT SHOULD RETURN THE CURRENT VALUE
		public ulong NextId() {
			lock (sync) {
				counter++;
				Write(counter);
				return counter;
			}
		}

		public void Dispose() {
			if (!disposed) {
				disposed = true;
				stream.Dispose();
				logger.LogInformation("Disposed DurableAtomicCounter({name})", this.fileName);
			}
		}
	}
}
