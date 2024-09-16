﻿using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient.Models {
	public record class ControllerInfo {
		public const string ControllerNamePlaceholder = "[controller]";
		public const string ControllerPostfix = "Controller";

		[JsonIgnore]
		public INamedTypeSymbol Controller { get; }
		public string ControllerName {
			get {
				if (this.Controller.Name.EndsWith(ControllerPostfix)) {
					return this.Controller.Name.Substring(0, this.Controller.Name.Length - ControllerPostfix.Length);
				} else {
					return this.Controller.Name;
				}
			}
		}
		public string Namespace { get; }
		public string Route { get; set; }
		public List<MethodInfo> Methods { get; } = new List<MethodInfo>();

		public ControllerInfo(CSharpProxyControllerSettings settings, Compilation compilation, INamedTypeSymbol controller) {
			this.Namespace = settings.Namespace;
			this.Controller = controller;
			this.Route = controller.GetRouteText();
			this.Route = this.Route.Replace(ControllerNamePlaceholder, this.ControllerName.ToLower());

			foreach (var methodSymbol in controller.GetMembers().OfType<IMethodSymbol>()) {
				if (methodSymbol.GetAttributes().Any(x => x.AttributeClass?.BaseType?.GetFullName() == My.HttpMethodAttributeClassName)) {
					Methods.Add(new MethodInfo(settings.Get(controller.Name, methodSymbol.Name), compilation, methodSymbol));
				}
			}
		}
	}
}
