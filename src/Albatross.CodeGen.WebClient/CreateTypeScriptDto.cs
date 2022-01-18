using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.WebClient {
	public interface ICreateTypeScriptDto {
		TypeScriptFile Generate(IEnumerable<Assembly> assemblies, IEnumerable<Type> additionalDtoClass, 
			string outputDirectory, Func<Type, bool>? isValidType);
	}
	public class CreateTypeScriptDto : ICreateTypeScriptDto {
		private readonly IConvertDtoToTypeScriptInterface converter;
		private readonly ILogger<CreateTypeScriptDto> logger;

		public CreateTypeScriptDto(IConvertDtoToTypeScriptInterface converter, ILogger<CreateTypeScriptDto> logger) {
			this.converter = converter;
			this.logger = logger;
		}

		public TypeScriptFile Generate(IEnumerable<Assembly> assemblies, IEnumerable<Type> additionalDtoClass, 
			string outputDirectory, Func<Type, bool>? isValidType) {

			string dtoFileName = System.IO.Path.Join(outputDirectory, "dto.ts");
			TypeScriptFile file = new TypeScriptFile(Path.GetFileNameWithoutExtension(dtoFileName));
			converter.ConvertEnums(file, assemblies.ToArray());
			foreach (var type in additionalDtoClass) {
				if (type.IsEnum) {
					this.converter.ConvertEnum(type, file);
				} else {
					this.converter.ConvertClass(type, file, new TypeScriptFile[0]);
				}
			}
			converter.ConvertClasses(file, new TypeScriptFile[0], isValidType, assemblies.ToArray());
			using (var writer = new StreamWriter(dtoFileName, false)) {
				writer.Code(file);
			}
			return file;
		}
	}
}
