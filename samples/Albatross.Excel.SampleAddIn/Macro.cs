using ExcelDna.Integration;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Albatross.Excel.SampleAddIn {
	public static class Macro {
		public static void Use<T>(this IServiceProvider provider) where T : class {
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
