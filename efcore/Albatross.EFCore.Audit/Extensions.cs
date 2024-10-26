using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.EFCore.Audit {
	public static class Extensions {
		public static IServiceCollection AddAuditEventHandlers(this IServiceCollection services) {
			services.TryAddEnumerable(ServiceDescriptor.Scoped<IDbSessionEventHandler, AuditChangeDbEventHandler>());
			return services;
		}
	}
}