using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.CSharp.Writer
{
    public class WriteCSharpClass : CodeGeneratorBase<Class> {
		ICodeGenerator<AccessModifier> writeAccessModifier;
		ICodeGenerator<Constructor> writeConstructor;
		ICodeGenerator<Method> writeMethod;
		ICodeGenerator<Field> writeField;
		ICodeGenerator<Property> writeProperty;

		public WriteCSharpClass(ICodeGenerator<AccessModifier> writeAccessModifier, ICodeGenerator<Constructor> writeConstructor, ICodeGenerator<Method> writeMethod, ICodeGenerator<Field> writeField, ICodeGenerator<Property> writeProperty) {
			this.writeAccessModifier = writeAccessModifier;
			this.writeConstructor = writeConstructor;
			this.writeMethod = writeMethod;
			this.writeField = writeField;
			this.writeProperty = writeProperty;
		}

        public override void Run(TextWriter writer, Class @class) {
			if (@class.Imports?.Count() > 0) {
				foreach(var item in @class.Imports) {
					writer.Append("using ").Append(item).AppendLine(";");
				}
				writer.WriteLine();
			}

			using (var scope = writer.BeginScope($"namespace {@class.Namespace}")) {
				scope.Writer.Run(writeAccessModifier, @class.AccessModifier);
				if (@class.Static) { scope.Writer.Append(" static"); }
				if (@class.Partial) { scope.Writer.Append(" partial"); }
				scope.Writer.Append(" class ").Append(@class.Name);
				if(@class.BaseClass != null) {
					scope.Writer.Append(" : ").Append(@class.BaseClass.Name);
				}

				using (var childScope = scope.Writer.BeginScope()) {
					if(@class.Constructors?.Count() > 0) {
						foreach(var constructor in @class.Constructors) {
                            childScope.Writer.Run(writeConstructor, constructor).WriteLine();
						}
					}
					if(@class.Fields?.Count() > 0) {
						foreach(var field in @class.Fields) {
                            childScope.Writer.Run(writeField, field).WriteLine();
                        }
					}
					if (@class.Properties?.Count() > 0) {
						foreach (var property in @class.Properties) {
                            childScope.Writer.Run(writeProperty, property).WriteLine();
						}
					}
					if (@class.Methods?.Count() > 0) {
						foreach (var method in @class.Methods) {
                            childScope.Writer.Run(writeMethod, method).WriteLine();
						}
					}
				}
			}
		}
	}
}
