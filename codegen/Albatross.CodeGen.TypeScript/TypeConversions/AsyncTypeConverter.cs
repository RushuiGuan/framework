﻿using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AsyncTypeConverter : ITypeConverter {
		public const string PromiseType = "Promise";
		public const string VoidType = "void";

		public int Precedence => 90;
		public bool Match(Type type) => type.IsConcreteType() && type.IsDerived<Task>();
		public Expression Convert(Type type, TypeConverterFactory factory, SyntaxTree syntaxTree) {
			if (type.IsGenericType) {
				return new GenericTypeExpressionBuilder()
					.WithName(PromiseType)
					.WithArgument(t => factory.Convert(syntaxTree, type.GetGenericArguments().First()))
					.Build(syntaxTree);
			} else {
				return new GenericTypeExpressionBuilder().WithName(PromiseType)
					.WithArgument(t => t.Type(VoidType))
					.Build(syntaxTree);
			}
		}
	}
}
