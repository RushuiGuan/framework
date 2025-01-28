using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient.Settings;
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
		public string Route { get; set; }
		public List<MethodInfo> Methods { get; } = new List<MethodInfo>();

		public ControllerInfo(CSharpWebClientSettings settings, Compilation compilation, INamedTypeSymbol controller) {
			this.Controller = controller;
			this.Route = controller.GetRouteText();
			this.Route = this.Route.Replace(ControllerNamePlaceholder, this.ControllerName.ToLower());

			foreach (var methodSymbol in controller.GetMembers().OfType<IMethodSymbol>()) {
				if (methodSymbol.GetAttributes().Any(x => x.AttributeClass?.BaseType?.GetFullName() == My.HttpMethodAttributeClassName)) {
					Methods.Add(new MethodInfo(settings.Get(controller.Name, methodSymbol.Name), compilation, methodSymbol));
				}
			}
		}
		public void ApplyMethodFilters(IEnumerable<Settings.SymbolFilter> filters) {
			if (filters.Any()) {
				var controllerName = this.Controller.GetFullName();
				for (int i = this.Methods.Count - 1; i >= 0; i--) {
					var method = this.Methods[i];
					if(!Settings.SymbolFilter.ShouldKeep(filters, $"{controllerName}.{method.Name}")) {
						this.Methods.RemoveAt(i);
					}
				}
			}
		}
	}
}