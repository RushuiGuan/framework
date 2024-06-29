﻿using Albatross.CodeGen.TypeScript.Models;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class DateTypeConverter: ITypeConverter {
		public int Precedence => 0;
		public TypeExpression Convert(Type type, TypeConverterFactory _, SyntaxTree syntaxTree) => TypeExpression.Date();
		public bool Match(Type type) => type == typeof(DateOnly) || type == typeof(DateTime) || type == typeof(TimeOnly) || type == typeof(DateTimeOffset);
	}
}
