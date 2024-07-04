using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AsyncTypeConverter : ITypeConverter {
		public const string PromiseType = "Promise";

		public int Precedence => 90;
		public bool Match(Type type) => type.IsConcreteType() && type.IsDerived<Task>();
		public ITypeExpression Convert(Type type, TypeConverterFactory factory) {
			if (type.IsGenericType) {
				return new GenericTypeExpression(PromiseType) {
					Arguments = new ListOfSyntaxNodes<ITypeExpression>(factory.Convert(type.GetGenericArguments().First()))
				};
			} else {
				return new GenericTypeExpression(PromiseType) {
					Arguments = new ListOfSyntaxNodes<ITypeExpression>(Defined.Types.Void)
				};
			}
		}
	}
}
