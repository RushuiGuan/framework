using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Templating {
	public static class Extension {
		public static IServiceCollection AddTemplating(this IServiceCollection services) {
			services.AddSingleton<IStringInterpolationService, StringInterpolationService>()
				.AddSingleton<IRazorTemplateService, RazorTemplateService>();
			return services;
		}
	}
}
