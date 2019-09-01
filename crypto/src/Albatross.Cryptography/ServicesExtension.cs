using Albatross.Cryptography.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Cryptography {
    public static class ServicesExtension {
		public static IServiceCollection AddCrypto(this IServiceCollection services) {
			services.AddSingleton<ICryptoRNG, CryptoRNG>();
			services.AddSingleton<ICreateHMACHash, CreateHMACSHAHash>();
			services.AddSingleton<CreateSHA256Hash>();
			services.AddSingleton<CreateSHA512Hash>();
			return services;
		}
	}
}