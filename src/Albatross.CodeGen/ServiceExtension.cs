using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Albatross.CodeGen {
	public static class ServiceExtension {
		public static IServiceCollection AddCodeGen(this IServiceCollection services, Assembly assembly) {
			services.AddSingleton<ICodeGenFactory, CodeGenFactory>();
			foreach (var type in assembly.GetTypes()) {
				TryAddCodeGenerator(services, type);
				TryAddConverter(services, type);
			}
			return services;
		}

		public static void RegisterDefault(this ICodeGenFactory factory) {
			factory.Register(typeof(CodeGenFactory).Assembly);
		}

		public static IServiceCollection AddDefaultCodeGen(this IServiceCollection services) {
			services.AddCodeGen(typeof(ServiceExtension).Assembly);
			return services;
		}

		public static bool TryAddCodeGenerator(this IServiceCollection services, Type codeGenType) {
			if (codeGenType.TryGetClosedGenericType(typeof(ICodeGenerator<>), out Type? genericInterfaceType)) {
				// register any ICodeGenerator
				services.AddTransient(codeGenType);
				services.AddTransient(genericInterfaceType, codeGenType);
				return true;
			} else {
				return false;
			}
		}

		public static bool TryAddConverter(this IServiceCollection services, Type converterType) {
			if (converterType.TryGetClosedGenericType(typeof(IConvertObject<>), out Type? genericInterfaceType)) {
				services.AddTransient(converterType);
				services.AddTransient(genericInterfaceType, converterType);
				if (converterType.TryGetClosedGenericType(typeof(IConvertObject<,>), out genericInterfaceType)) {
					services.AddTransient(genericInterfaceType, converterType);
				}
				return true;
			} else {
				return false;
			}
		}

		public static ICodeGenFactory RunCodeGen(this ICodeGenFactory factory, TextWriter writer,  object model) {
			if (model != null) {
				factory.Get(model.GetType()).Run(writer, model);
			}
			return factory;
		}
	}
}

