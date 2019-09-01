using Albatross.Host.NUnit;
using Albatross.WebClient.IntegrationTest.Messages;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Albatross.WebClient.IntegrationTest {
    [TestFixture]
	public class TestValueClientService :TestBase<TestUnitOfWork>{
		public override void RegisterPackages(IServiceCollection svc) {
            svc.WithTestClientService(client => {
                client.BaseAddress = new Uri("http://localhost:20000");
            });
		}

		[Test]
		public async Task TestGetText() {
			using (var unitOfWork = NewUnitOfWork()) {
				var result = await unitOfWork.Get<ValueClientService>().GetText();
				Assert.NotNull(result);
                Assert.AreEqual(PayLoadExtension.GetText(), result);
			}
		}
        [Test]
        public async Task TestGetJson() {
            using (var unitOfWork = NewUnitOfWork()) {
                var result = await unitOfWork.Get<ValueClientService>().GetJson();
                Assert.NotNull(result);
                var expected = PayLoadExtension.Make();
                Assert.AreEqual(expected.Data, result.Data);
                Assert.AreEqual(expected.Date, result.Date);
                Assert.AreEqual(expected.DateTimeOffset, result.DateTimeOffset);
                Assert.AreEqual(expected.Name, result.Name);
                Assert.AreEqual(expected.Number, result.Number);
            }
        }

        [Test]
        public async Task TestPostJson() {
            using (var unitOfWork = NewUnitOfWork()) {
                var result = await unitOfWork.Get<ValueClientService>().Post(PayLoadExtension.Make());
                Assert.NotNull(result);
                var expected = PayLoadExtension.Make();
                Assert.AreEqual(expected.Data, result.Data);
                Assert.AreEqual(expected.Date, result.Date);
                Assert.AreEqual(expected.DateTimeOffset, result.DateTimeOffset);
                Assert.AreEqual(expected.Name, result.Name);
                Assert.AreEqual(expected.Number, result.Number);
            }
        }
    }
}
