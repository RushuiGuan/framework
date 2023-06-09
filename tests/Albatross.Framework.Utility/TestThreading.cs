using CommandLine;
using System;
using System.Threading.Tasks;
using Albatross.Hosting.Utility;

namespace Albatross.Framework.Utility {
	[Verb("test-threading")]
	public class TestThreadingOption : BaseOption { }
	public class TestThreading : UtilityBase<TestThreadingOption> {
		public TestThreading(TestThreadingOption option) : base(option) {
		}
		public async Task<int> RunUtility() {
			Options.WriteOutput($"main thread1: {Environment.CurrentManagedThreadId}");
			try {
				_ = IFail();
			} catch {
				Options.WriteOutput($"caught 1: {nameof(IFail)}, thread: {Environment.CurrentManagedThreadId}");
			}
			Console.WriteLine("if the async call has no async code, it executes the same as non async call.  press enter to continue:");
			Console.ReadLine();

			try {
				await IFail();
			} catch {
				Options.WriteOutput($"caught 2: {nameof(IFail)}, thread: {Environment.CurrentManagedThreadId}");
			}
			Console.WriteLine("it doesn't matter if the call is awaited or not, no new thread is created by this.");
			Console.ReadLine();

			Options.WriteOutput($"main thread2: {Environment.CurrentManagedThreadId}");
			_ = Task.Run(() => IFail());
			Options.WriteOutput($"main thread3: {Environment.CurrentManagedThreadId}");
			Console.WriteLine("unless the method is explicitly called using Task.Run.  This garantees a different thread from caller.  This call now behaves like an async void.  The exception is not caught and the returns is lost");
			Console.ReadLine();

			Console.Write("now same example as above with no exceptions");
			Options.WriteOutput($"main thread4: {Environment.CurrentManagedThreadId}");
			Console.Write("Press enter to continue");
			await IDontFail();
			Options.WriteOutput($"main thread5: {Environment.CurrentManagedThreadId}");
			Console.WriteLine("Press enter to continue");
			Console.ReadLine();
			
			_ = IDontFail();
			Options.WriteOutput($"main thread4: {Environment.CurrentManagedThreadId}");
			Console.WriteLine("Press enter to continue");
			Console.ReadLine();

			_ = Task.Run(() => IDontFail());
			Options.WriteOutput($"main thread5: {Environment.CurrentManagedThreadId}");
			Console.WriteLine("Press enter to continue");
			Console.ReadLine();

			Options.WriteOutput($"main thread6: {Environment.CurrentManagedThreadId}");
			_ = IDontFailJustALittleSlower(1);
			Options.WriteOutput($"main thread7: {Environment.CurrentManagedThreadId}");
			Console.WriteLine("This async call actually has async code.  the code up to the async code runs on the caller thread and the code afterwards runs on a different thread.  since caller didn't await, response is lost");
			Console.ReadLine();

			Options.WriteOutput($"main thread8: {Environment.CurrentManagedThreadId}");
			_ = IFailALittleSlower(2);
			Options.WriteOutput($"main thread9: {Environment.CurrentManagedThreadId}");
			Console.WriteLine("same example as above but an exception is throw after the async code, note that exception is not caught since caller didn't await");
			Console.ReadLine();

			Options.WriteOutput($"main thread10: {Environment.CurrentManagedThreadId}");
			await IDontFailJustALittleSlower(2);
			Options.WriteOutput($"main thread11: {Environment.CurrentManagedThreadId}");
			Console.WriteLine("now the async code is awaited, the code prior the async code will run on the caller thread, the code after the async code will run on a new thread.  the caller will also resume on the same new thread");
			Console.ReadLine();



			Console.WriteLine("The end of sample code");
			Console.ReadLine();
			return 0;
		}


		Task IFail() {
			Options.WriteOutput($"{nameof(IFail)}, thread:{Environment.CurrentManagedThreadId}");
			throw new System.Exception("failure");
		}

		Task IDontFail() {
			Options.WriteOutput($"{nameof(IDontFail)}, thread:{Environment.CurrentManagedThreadId}");
			return Task.CompletedTask;
		}

		async Task IDontFailJustALittleSlower(int id) {
			Options.WriteOutput($"{nameof(IDontFailJustALittleSlower)}({id}) before sleep, thread: {Environment.CurrentManagedThreadId}");
			await Task.Delay(1000);
			Options.WriteOutput($"{nameof(IDontFailJustALittleSlower)}({id}) after sleep, thread: {Environment.CurrentManagedThreadId}");
		}
		async Task IFailToo() {
			Options.WriteOutput($"{nameof(IFailToo)} before async, thread: {Environment.CurrentManagedThreadId}");
			await IDontFail();
			Options.WriteOutput($"{nameof(IFailToo)} after async, thread: {Environment.CurrentManagedThreadId}");
			throw new System.Exception("failure");
		}

		async Task IFailALittleSlower(int id) {
			Options.WriteOutput($"{nameof(IFailALittleSlower)}({id}) before async, thread: {Environment.CurrentManagedThreadId}");
			await IDontFailJustALittleSlower(id);
			Options.WriteOutput($"{nameof(IFailALittleSlower)}({id}) after async, thread: {Environment.CurrentManagedThreadId}");
			throw new System.Exception("failure");
		}
	}
}
