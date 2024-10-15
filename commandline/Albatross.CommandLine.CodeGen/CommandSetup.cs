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
				this.Name = VerbAttribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
			} else {
				this.Name = "MissingVerbName";
			}
			if (verbAttribute.ConstructorArguments.Length > 1) {
				this.HandlerClass = verbAttribute.ConstructorArguments[1].Value?.ToString() ?? string.Empty;
			} else {
				this.HandlerClass = "MissingHandlerClassName";
			}
			if (VerbAttribute.TryGetNamedArgument("Description", out var description)) {
				this.Description = description.Value?.ToString();
			}
			if (VerbAttribute.TryGetNamedArgument("Alias", out var verbAlias)) {
				this.Aliases = verbAlias.Values.Select(x => x.Value?.ToString() ?? string.Empty).ToArray();
			}
			this.Options = GetCommandOptions();
		}


		public INamedTypeSymbol OptionClass { get; }
		public AttributeData VerbAttribute { get; }
		public string HandlerClass { get; }
		public string CommandClassName { get; private set; }
		public string Name { get; }
		public string? Description { get; }
		public string[] Aliases { get; } = Array.Empty<string>();
		public CommandOptionSetup[] Options { get; private set; }

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
		public CommandOptionSetup[] GetCommandOptions() {
			var propertySymbols = OptionClass.GetMembers().OfType<IPropertySymbol>()
				.Where(s => s.SetMethod?.DeclaredAccessibility == Accessibility.Public
					&& s.GetMethod?.DeclaredAccessibility == Accessibility.Public)
				.ToList();
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
