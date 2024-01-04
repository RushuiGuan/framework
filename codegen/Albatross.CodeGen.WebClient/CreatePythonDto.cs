using Albatross.CodeGen.Python.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.WebClient {
	public interface ICreatePythonDto {
		PythonModule Generate(IEnumerable<Assembly> assemblies, IEnumerable<Type> additionalDtoClass, IEnumerable<PythonModule> dependancies,
			string outputDirectory, string name, Func<Type, bool>? isValidType);
	}
	public class CreatePythonDto : ICreatePythonDto {
		private readonly IConvertDtoToPythonClass converter;
		private readonly ILogger<CreatePythonDto> logger;

		public CreatePythonDto(IConvertDtoToPythonClass converter, ILogger<CreatePythonDto> logger) {
			this.converter = converter;
			this.logger = logger;
		}

		public PythonModule Generate(IEnumerable<Assembly> assemblies, IEnumerable<Type> additionalDtoClass, IEnumerable<PythonModule> dependancies,
			string outputDirectory, string name, Func<Type, bool>? isValidType) {

			string dtoFileName = System.IO.Path.Join(outputDirectory, $"{name}.py");
			PythonModule module = new PythonModule(name);
			converter.ConvertEnums(module, assemblies.ToArray());
			foreach (var type in additionalDtoClass) {
				if (type.IsEnum) {
					this.converter.ConvertEnum(type, module);
				} else {
					this.converter.ConvertClass(type,  module);
				}
			}
			converter.ConvertClasses(module, isValidType, assemblies.ToArray());
			using (var writer = new StreamWriter(dtoFileName, false)) {
				writer.Code(module);
			}
			return module;
		}
	}
}
