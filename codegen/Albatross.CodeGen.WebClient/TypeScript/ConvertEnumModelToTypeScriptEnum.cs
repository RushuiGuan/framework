using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.WebClient.Models;
using System.Linq;

namespace Albatross.CodeGen.CommandLine {
	public class ConvertEnumModelToTypeScriptEnum : IConvertObject<EnumInfo, EnumDeclaration> {
		public EnumDeclaration Convert(EnumInfo from) {
			return new EnumDeclaration(from.Name) {
				Items = new ListOfEnumItems(
					from.Members.Select(x => new EnumItemExpression(x.Name) {
						Expression = from.UseTextAsValue ? new StringLiteralExpression(x.Name) : new NumberLiteralExpression(x.NumericValue),
					})
				),
			};
		}
		object IConvertObject<EnumInfo>.Convert(EnumInfo from) => this.Convert(from);
	}
}