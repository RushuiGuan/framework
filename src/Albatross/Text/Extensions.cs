using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.Text {
	public static class Extensions {
		public static IServiceCollection AddStringInterpolation(this IServiceCollection services) {
			services.TryAddSingleton<IStringInterpolationService, StringInterpolationService>();
			return services;
		}

		public static bool TryGetText(this string line, char delimiter, ref int offset, [NotNullWhen(true)] out string? text) {
			if (offset == line.Length + 1) {
				text = null;
				return false;
			} else {
				var index = line.IndexOf(delimiter, offset);
				if (index == -1) {
					text = line.Substring(offset);
					offset = line.Length + 1;
					return true;
				} else {
					text = line.Substring(offset, index - offset);
					offset = index + 1;
					return true;
				}
			}
		}
	}
}