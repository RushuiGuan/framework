using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.Text {
	public static class Extensions {
		public static IServiceCollection AddStringInterpolation(this IServiceCollection services) {
			services.TryAddSingleton<IStringInterpolationService, StringInterpolationService>();
			return services;
		}
	}
}