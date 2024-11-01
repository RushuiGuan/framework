using ExcelDna.Integration.CustomUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Albatross.DependencyInjection.Excel {
	public static class Extensions {
		public static IServiceCollection AddExcelRibbon<T>(this IServiceCollection services) where T : HostedExcelRibbon {
			services.TryAddEnumerable(ServiceDescriptor.Singleton<ExcelRibbon, T>());
			return services;
		}
		public static Type GetDelegateType(this MethodInfo methodInfo) {
			Func<Type[], Type> getType;
			var isAction = methodInfo.ReturnType == typeof(void);
			var types = methodInfo.GetParameters().Select(p => p.ParameterType);
			if (isAction) {
				getType = Expression.GetActionType;
			} else {
				getType = Expression.GetFuncType;
				types = types.Concat(new[] { methodInfo.ReturnType });
			}
			return getType(types.ToArray());
		}

		public static void UseExcelFunctions<T>(this IServiceProvider provider) where T : class {
			var service = provider.GetRequiredService<FunctionRegistrationService>();
			service.RegisterExcelFunctions<T>();
		}
	}
}