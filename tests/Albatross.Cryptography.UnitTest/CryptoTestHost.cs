using Albatross.Host.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Cryptography.UnitTest {
	public class CryptoTestHost :TestHost{
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCrypto();
			services.AddSingleton<CreateHMACSHAHash>();
		}
	}
}
