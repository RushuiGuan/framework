using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Host.Test {
	public class AsyncTest {
		public interface ITestDisposable : IDisposable {
			void Run();
		}

		[Fact]
		public async Task Run() {
			Mock<ITestDisposable> mock = new Mock<ITestDisposable>(MockBehavior.Strict);
			MockSequence sequence = new MockSequence();
			mock.InSequence(sequence).Setup(args => args.Dispose());
			mock.InSequence(sequence).Setup(args => args.Run());
			var handle = mock.Object;
			using (handle) {
				await Task.Delay(5000);
			}
			handle.Run();
			mock.VerifyAll();
		}
	}
}
