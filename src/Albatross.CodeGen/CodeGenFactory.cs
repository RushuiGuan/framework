using Albatross.CodeGen.Core;
using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Albatross.CodeGen {
	public interface ICodeGenFactory {
		void Register(Assembly assembly, IServiceProvider provider);
		ICodeGenerator Get(Type type);
	}
	public class CodeGenFactory: ICodeGenFactory {
		Dictionary<Type, ICodeGenerator> dict = new Dictionary<Type, ICodeGenerator>();
		
		public ICodeGenerator Get(Type type) {
			if(dict.TryGetValue(type, out var codeGenerator)) {
				return codeGenerator;
			} else {
				throw new InvalidOperationException($"Code generator for {type.FullName} is not registered");
			}
		}

		public void Register(Assembly assembly, IServiceProvider provider) {
			foreach (var type in assembly.GetTypes()) {
				if (type.TryGetClosedGenericType(typeof(ICodeGenerator<>), out Type? genericInterfaceType)) {
					if (genericInterfaceType.IsAssignableTo(typeof(ICodeBlock))) {
						ICodeGenerator codeGenerator = (ICodeGenerator)provider.GetRequiredService(type);
						dict[codeGenerator.Type] = codeGenerator;
					}
				}
			}
		}
	}
}
