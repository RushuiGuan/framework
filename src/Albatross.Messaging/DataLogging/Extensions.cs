using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.Messaging.DataLogging {
	public static class Extensions {
		public static IServiceCollection AddNoOpDataLogging(this IServiceCollection services) {
			services.TryAddSingleton<IDataLogReader, NoOpDataLogReader>();
			services.TryAddSingleton<IDataLogWriter, NoOpDataLogWriter>();
			return services;
		}
	}
}
