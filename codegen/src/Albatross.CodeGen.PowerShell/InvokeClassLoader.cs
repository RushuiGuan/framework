using Albatross.CodeGen.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.PowerShell {
	[Cmdlet(VerbsLifecycle.Invoke, "ClassLoader")]
	public class InvokeClassLoader:Cmdlet  {
		[Parameter(Mandatory = true, HelpMessage = "Target assembly file")]
		[Alias("t")]
		public string TargetAssembly { get; set; }


		[Parameter(HelpMessage = "Comma delimited referenced assembly paths")]
		[Alias("a")]
		public string[] AssemblyFolders { get; set; }

		[Parameter(HelpMessage = "Regular expression pattern")]
		[Alias("p")]
		public string Pattern { get; set; }

		[Parameter(HelpMessage = "Specify the namespaces of the target types")]
		[Alias("n")]
		public string[] Namespaces { get; set; }

		[Parameter(Mandatory = false, HelpMessage = "Specify the converter used to convert types, by default it is the ConvertTypeToCSharpClass")]
		[Alias("c")]
		public string Converter { get; set; } = nameof(Albatross.CodeGen.CSharp.Conversion.ConvertTypeToCSharpClass);

		protected override void ProcessRecord() {
			ServiceCollection services = new ServiceCollection();
			services.AddDefaultCodeGen();
			HashSet<string> paths = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
			paths.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
			paths.AddRange(AssemblyFolders);

			var getAssemblyTypes = new GetAssemblyTypes(paths);
			Type converterType = getAssemblyTypes.LoadTypeByName(Converter);
			if (!services.TryAddConverter(converterType)) {
				throw new Exception($"codegen: Error loading converter type: ${converterType}");
			}
			var provider = services.BuildServiceProvider();


			Regex regex = null;
			IConvertObject<Type> converter = provider.GetRequiredService(converterType) as IConvertObject<Type>;
			if (!string.IsNullOrEmpty(Pattern)) {
				regex = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			}

			getAssemblyTypes.LoadAssemblyTypes(TargetAssembly, type => {
				if (!type.IsAnonymousType() && !type.IsInterface && type.IsPublic) {
					bool match = (Namespaces?.Count() ?? 0) == 0 || Namespaces.Contains(type.Namespace, StringComparer.InvariantCultureIgnoreCase);
					if (match) {
						match = match && (regex == null || regex.IsMatch(type.FullName));
					}
					if (match) {
						var data = converter.Convert(type);
						WriteObject(data);
					}
				}
			});
		}
	}
}
