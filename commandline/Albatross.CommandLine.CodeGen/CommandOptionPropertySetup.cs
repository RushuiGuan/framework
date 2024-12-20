﻿using Albatross.CodeAnalysis.Symbols;
using Humanizer;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Albatross.CommandLine.CodeGen {
	public class CommandOptionPropertySetup : CommandPropertySetup {
		public CommandOptionPropertySetup(IPropertySymbol property, AttributeData? propertyAttribute)
			: base(property, propertyAttribute, $"--{property.Name.Kebaberize()}") {
			this.Aliases = Array.Empty<string>();
			if (propertyAttribute != null) {
				if (propertyAttribute.ConstructorArguments.Any()) {
					this.Aliases = propertyAttribute.ConstructorArguments[0].Values.Select(x => x.Value?.ToString() ?? string.Empty)
						.Where(x => !string.IsNullOrEmpty(x))
						.ToArray();
				}
				if (propertyAttribute.TryGetNamedArgument("Required", out var required)) {
					this.Required = Convert.ToBoolean(required.Value);
				}
			}
			this.Required = property.Type.SpecialType != SpecialType.System_Boolean && !property.Type.IsNullable() && !property.Type.IsCollection() && !ShouldDefaultToInitializer;
		}

		public override string CommandPropertyName => $"Option_{this.Property.Name}";
		public bool Required { get; private set; }
		public string[] Aliases { get; }
	}
}