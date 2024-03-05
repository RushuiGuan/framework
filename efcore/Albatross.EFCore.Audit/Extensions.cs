using Microsoft.Extensions.DependencyInjection;

namespace Albatross.EFCore.Audit {
	public static class Extensions {
		public static IServiceCollection AddAuditEventHandlers(this IServiceCollection services) {
			services.AddScoped<IDbChangeEventHandler, AuditDbSessionEventHandler>();
			return services;
		}
	}
}
