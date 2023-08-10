using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Albatross.Messaging.Services {
	public class DurableAtomicCounter : IAtomicCounter<ulong>, IDisposable{
		private readonly ILogger logger;
		Stream stream;
		string fileName;
		ulong counter;
		bool disposed = false;
		object sync = new object();
		public ulong Counter => counter;	

		public DurableAtomicCounter(string directory, string name, ILogger logger) {
			fileName = Path.Join(directory, $"{name}");
			this.stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
			logger.LogInformation("Start DurableAtomicCounter using file {name}", fileName);
			this.counter = Read();
			this.logger = logger;
		}

		ulong Read() {
			if(stream.Length == 0) {
				return 0;
			} else {
				try {
					byte[] buffer = new byte[sizeof(ulong)];
					stream.ReadExactly(buffer, 0, sizeof(ulong));
					return BitConverter.ToUInt64(buffer, 0);
				}catch(Exception err) {
					logger.LogError(err, "DurableAtomicCounter: eror reading from {name}, resetting to 0", this.fileName);
					return 0;
				}
			}
		}
		void Write(ulong data) {
			stream.Seek(0, SeekOrigin.Begin);
			var bytes = BitConverter.GetBytes(data);
			stream.Write(bytes, 0, bytes.Length);
			stream.Flush();
		}

		public ulong NextId() {
			lock (sync) {
				Write(++ counter);
				return counter;
			}
		}

		public void Set(ulong value) {
			lock (sync) {
				counter = value;
				Write(value);
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
