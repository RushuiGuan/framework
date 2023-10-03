using System;
using Albatross.Hosting.Test;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Albatross.Cryptography.UnitTest {
    public class Tests : IClassFixture<CryptoTestHost> {
		private readonly CryptoTestHost host;

		public Tests(CryptoTestHost host) {
			this.host = host;
		}

		[Theory]
		[InlineData("")]
		[InlineData("test")]
		[InlineData("123")]
		public void TestCreate512BitHash(string secret) {
			using (var scope = host.Create()) {
				var createHash = scope.Get<CreateHMACSHAHash>();
				byte[] hash, salt;
				hash = createHash.Create512(secret, out salt);
				byte[] hash2 = createHash.Create(secret, salt);
				Assert.Equal(hash, hash2);
			}
		}

		[Theory]
        [InlineData("")]
		[InlineData("test")]
		[InlineData("123")]
		public void TestCreate256BitHash(string secret) {
			using (var scope = host.Create()) {
				var createHash = scope.Get<CreateHMACSHAHash>();
				byte[] hash, salt;
				hash = createHash.Create256(secret, out salt);
				byte[] hash2 = createHash.Create(secret, salt);
				Assert.Equal(hash, hash2);
			}
		}

		[Fact]
		public void TestCreateHash_NullCheck() {
            using (var scope = host.Create()) {
				Action action = () => {
					var createHash = scope.Get<CreateHMACSHAHash>();
					byte[] hash, salt;
					hash = createHash.Create256(null, out salt);
				};
				Assert.Throws<ArgumentNullException>(action);
            }
		}
	}
}