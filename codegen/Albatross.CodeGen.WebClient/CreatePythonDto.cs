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
			string outputDirectory, string name, Func<Type, bool>? isValidType, bool useDataClass);
	}
	public class CreatePythonDto : ICreatePythonDto {
		private readonly IConvertDtoToPythonClass classConverter;
		private readonly ILogger<CreatePythonDto> logger;

		public CreatePythonDto(IConvertDtoToPythonClass classConverter, ILogger<CreatePythonDto> logger) {
			this.classConverter = classConverter;
			this.logger = logger;
		}

		public PythonModule Generate(IEnumerable<Assembly> assemblies, IEnumerable<Type> additionalDtoClass, 
			IEnumerable<PythonModule> dependancies, string outputDirectory, string name, Func<Type, bool>? isValidType, bool useDataClass) {

			string dtoFileName = System.IO.Path.Join(outputDirectory, $"{name}.py");
			PythonModule module = new PythonModule(name);
			classConverter.ConvertEnums(module, assemblies.ToArray());
			foreach (var type in additionalDtoClass) {
				if (type.IsEnum) {
					this.classConverter.ConvertEnum(type, module);
				} else {
					this.classConverter.ConvertClass(type,  module, useDataClass);
				}
			}
			classConverter.ConvertClasses(module, isValidType, useDataClass, assemblies.ToArray());
			module.Build();
			using (var writer = new StreamWriter(dtoFileName, false)) {
				writer.Code(module);
			}
			return module;
		}
	}
}
