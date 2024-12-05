using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CommandLine.CodeGen {
	public class CommandSetup {
		public CommandSetup(INamedTypeSymbol optionClass, AttributeData verbAttribute) {
			this.OptionClass = optionClass;
			this.VerbAttribute = verbAttribute;
			this.CommandClassName = GetCommandClassName();

			if (verbAttribute.ConstructorArguments.Length > 0) {
				this.Key = VerbAttribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
			} else {
				this.Key = "MissingVerbKey";
			}
			this.Name = this.Key.Split(' ').Last();
			if (verbAttribute.ConstructorArguments.Length > 1) {
				this.HandlerClass = verbAttribute.ConstructorArguments[1].Value?.ToString() ?? string.Empty;
			} else {
				this.HandlerClass = string.Empty;
			}
			if (string.IsNullOrEmpty(this.HandlerClass)) {
				this.HandlerClass = "Albatross.CommandLine.HelpCommandHandler";
			}
			if (VerbAttribute.TryGetNamedArgument("Description", out var typedConstant)) {
				this.Description = typedConstant.Value?.ToString();
			}
			if (VerbAttribute.TryGetNamedArgument("Alias", out typedConstant)) {
				this.Aliases = typedConstant.Values.Select(x => x.Value?.ToString() ?? string.Empty).ToArray();
			}
			var useBaseClasssProperties = true;
			if (verbAttribute.TryGetNamedArgument("UseBaseClassProperties", out typedConstant)) {
				useBaseClasssProperties = Convert.ToBoolean(typedConstant.Value);
			}
			this.Options = GetCommandOptions(useBaseClasssProperties);
		}

		public string Key { get; set; }
		public string Name { get; }
		public INamedTypeSymbol OptionClass { get; }
		public AttributeData VerbAttribute { get; }
		public string HandlerClass { get; }
		public string CommandClassName { get; private set; }
		public string? Description { get; }
		public string[] Aliases { get; } = Array.Empty<string>();
		public CommandOptionSetup[] Options { get; private set; }

		/// <summary>
		/// Command class name is derived from the options class name by:
		/// 1. Remove the postfix "Options" if exists
		/// 2. Append "Command" if the remaining string does not end with "Command"
		/// </summary>
		public string GetCommandClassName() {
			string optionsClassName = OptionClass.Name;
			if (optionsClassName.EndsWith(My.Postfix_Options, StringComparison.InvariantCultureIgnoreCase)) {
				optionsClassName = optionsClassName.Substring(0, optionsClassName.Length - My.Postfix_Options.Length);
			}
			if (!optionsClassName.EndsWith(My.CommandClassName, StringComparison.InvariantCultureIgnoreCase)) {
				optionsClassName = optionsClassName + My.CommandClassName;
			}
			return optionsClassName;
		}
		public void RenameCommandClass(int index) {
			if (index != 0) {
				this.CommandClassName = $"{GetCommandClassName()}{index}";
			}
		}


		public CommandOptionSetup[] GetCommandOptions(bool useBaseClassProperties) {
			var propertySymbols = OptionClass.GetDistinctProperties(useBaseClassProperties).ToArray();
			var list = new List<CommandOptionSetup>();
			foreach (var propertySymbol in propertySymbols) {
				AttributeData? attributeData = null;
				var skip = false;
				if (propertySymbol.TryGetAttribute(My.OptionAttributeClass, out attributeData)) {
					if (attributeData!.TryGetNamedArgument("Skip", out var result)) {
						skip = Convert.ToBoolean(result.Value);
					}
				}
				if (!skip) {
					list.Add(new CommandOptionSetup(propertySymbol, attributeData));
				}
			}
			return list.ToArray();
		}
	}
}