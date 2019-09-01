using System;
using Albatross.Host.NUnit;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Albatross.Cryptography.UnitTest {
    public class Tests : TestBase<TestUnitOfWork>{
        public override void RegisterPackages(IServiceCollection services) {
            services.AddCrypto();
			services.AddSingleton<CreateHMACSHAHash>();
        }

		[TestCase("")]
		[TestCase("test")]
		[TestCase("123")]
		public void TestCreate512BitHash(string secret) {
			using (var unitOfWork = NewUnitOfWork()) {
				var createHash = unitOfWork.Get<CreateHMACSHAHash>();
				byte[] hash, salt;
				hash = createHash.Create512(secret, out salt);
				byte[] hash2 = createHash.Create(secret, salt);
				Assert.AreEqual(hash, hash2);
			}
		}

        [TestCase("")]
		[TestCase("test")]
		[TestCase("123")]
		public void TestCreate256BitHash(string secret) {
			using (var unitOfWork = NewUnitOfWork()) {
				var createHash = unitOfWork.Get<CreateHMACSHAHash>();
				byte[] hash, salt;
				hash = createHash.Create256(secret, out salt);
				byte[] hash2 = createHash.Create(secret, salt);
				Assert.AreEqual(hash, hash2);
			}
		}

		[Test]
		public void TestCreateHash_NullCheck() {
            using (var unitOfWork = NewUnitOfWork()) {
                TestDelegate testDelegate = new TestDelegate(() => {
                    var createHash = unitOfWork.Get<CreateHMACSHAHash>();
                    byte[] hash, salt;
                    hash = createHash.Create256(null, out salt);
                });
                Assert.Catch<ArgumentNullException>(testDelegate);
            }
		}
	}
}