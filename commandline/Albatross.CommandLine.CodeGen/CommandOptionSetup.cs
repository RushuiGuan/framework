using Albatross.CodeAnalysis.Symbols;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Albatross.CommandLine.CodeGen {
	public class CommandOptionSetup {
		public CommandOptionSetup(IPropertySymbol property, AttributeData? optionAttribute) {
			this.Property = property;
			this.PropertyInitializer = GetPropertyInitializer(property);
			this.OptionAttribute = optionAttribute;
			this.Name = $"--{property.Name.Kebaberize()}";
			this.Type = property.Type.ToDisplayString();
			this.Hidden = false;
			this.Description = null;
			this.Aliases = Array.Empty<string>();
			if (optionAttribute != null) {
				if (optionAttribute.ConstructorArguments.Any()) {
					this.Aliases = optionAttribute.ConstructorArguments[0].Values.Select(x => x.Value?.ToString() ?? string.Empty)
						.Where(x => !string.IsNullOrEmpty(x))
						.ToArray();
				}
				if (optionAttribute.TryGetNamedArgument("Required", out var required)) {
					this.Required = Convert.ToBoolean(required.Value);
				}
				if (optionAttribute.TryGetNamedArgument("DefaultToInitializer", out var defaultToInitializer)) {
					this.DefaultToInitializer = Convert.ToBoolean(defaultToInitializer.Value);
				}
				if (optionAttribute.TryGetNamedArgument("Hidden", out var hidden)) {
					this.Hidden = Convert.ToBoolean(hidden.Value);
				}
				if (optionAttribute.TryGetNamedArgument("Description", out var descriptionConstant)) {
					this.Description = descriptionConstant.Value?.ToString();
				}
			}
			this.Required = property.Type.SpecialType != SpecialType.System_Boolean && !property.Type.IsNullable() && !property.Type.IsCollection() && !ShouldDefaultToInitializer;
		}

		public string CommandOptionPropertyName => $"Option_{this.Property.Name}";
		public IPropertySymbol Property { get; }
		public ExpressionSyntax? PropertyInitializer { get; }
		public bool DefaultToInitializer { get; }
		public AttributeData? OptionAttribute { get; }
		public bool Required { get; private set; }
		public string Name { get; }
		public string Type { get; private set; }
		public string? Description { get; private set; }
		public bool Hidden { get; set; }
		public string[] Aliases { get; }
		public bool ShouldDefaultToInitializer => DefaultToInitializer && PropertyInitializer != null;

		static ExpressionSyntax? GetPropertyInitializer(IPropertySymbol propertySymbol) {
			foreach (var syntaxReference in propertySymbol.DeclaringSyntaxReferences) {
				var syntaxNode = syntaxReference.GetSyntax();
				if (syntaxNode is PropertyDeclarationSyntax propertyDeclaration) {
					return propertyDeclaration.Initializer?.Value;
				}
			}
			return null;
		}
	}
}