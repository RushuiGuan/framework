using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Albatross.Hosting.Excel {
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

		//public static void UseExcelFunctions<T>(this IServiceProvider provider) where T : class {
		//	var delegates = new List<Delegate>();
		//	var methodAttributes = new List<object>();
		//	var argumentAttributes = new List<List<object?>>();

		//	foreach (var methodInfo in typeof(T).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | BindingFlags.Static)) {
		//		var funcAttribute = methodInfo.GetCustomAttribute<ExcelFunctionAttribute>();
		//		if (funcAttribute != null) {
		//			if(string.IsNullOrEmpty(funcAttribute.Name)) {
		//				funcAttribute.Name = methodInfo.Name;
		//			}
		//			methodAttributes.Add(funcAttribute);
		//			var delegateType = methodInfo.GetDelegateType();
		//			if (methodInfo.IsStatic) {
		//				delegates.Add(Delegate.CreateDelegate(delegateType, methodInfo));
		//			} else {
		//				var target = provider.GetRequiredService<T>();
		//				delegates.Add(Delegate.CreateDelegate(delegateType, target, methodInfo));
		//			}
		//			argumentAttributes.Add(methodInfo.GetParameters().Select(args => {
		//				var attrib = args.GetCustomAttribute<ExcelArgumentAttribute>();
		//				if (attrib != null && string.IsNullOrEmpty(attrib.Name)) {
		//					attrib.Name = args.Name;
		//				}
		//				return attrib;
		//			}).ToList<object?>());
		//		}
		//	}
		//	argumentAttributes.Add(new List<object?>());
		//	ExcelIntegration.RegisterDelegates(delegates, methodAttributes, argumentAttributes);
		//}

		public static void UseExcelFunctions<T>(this IServiceProvider provider) where T : class {
			var service = provider.GetRequiredService<T>();
			List<ExcelFunctionRegistration> registrations = new List<ExcelFunctionRegistration>();
			foreach (var item in typeof(T).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)) {
				var attrib = item.GetCustomAttribute<ExcelFunctionAttribute>();
				if (attrib != null) {
					if (item.IsStatic) {
						registrations.Add(new ExcelFunctionRegistration(item));
					} else {
						var registration = CreateInstanceFunctionRegistration(service, item, attrib);
						registrations.Add(registration);
					}
				}
			}
			registrations.ProcessAsyncRegistrations().RegisterFunctions();
		}

		static ExcelFunctionRegistration CreateInstanceFunctionRegistration<T>(T service, MethodInfo methodInfo, ExcelFunctionAttribute functionAttribute) where T : class {
			if (string.IsNullOrEmpty(functionAttribute.Name)) { functionAttribute.Name = methodInfo.Name; }
			List<ExcelParameterRegistration> parameterRegistrations = new List<ExcelParameterRegistration>();
			List<ParameterExpression> list = new List<ParameterExpression>();
			foreach (var parameter in methodInfo.GetParameters()) {
				parameterRegistrations.Add(new ExcelParameterRegistration(parameter));
				list.Add(Expression.Parameter(parameter.ParameterType, parameter.Name));
			}
			var lambda = Expression.Lambda(Expression.Call(Expression.Constant(service), methodInfo, list), methodInfo.Name, list);
			return new ExcelFunctionRegistration(lambda, functionAttribute, parameterRegistrations);
		}
	}
}