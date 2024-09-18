using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Albatross.CodeGen.WebClient.Models {
	public record class DtoClassInfo {
		public DtoClassInfo(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
	}
	public record class PropertyInfo {
		public PropertyInfo(string name, ITypeSymbol type) {
			this.Name = name;
			this.Type = type;
		}
		public string Name { get; set; }
		[JsonIgnore]
		public ITypeSymbol Type { get; set; }
		public string TypeName => Type.GetFullName();
	}
}
