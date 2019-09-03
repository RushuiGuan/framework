using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.Database;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Conversion {
	public class ConvertStoredProcedureToDapperClass : IConvertObject<Procedure, Class> {
		ConvertSqlTypeToDotNetType getDotNetType;

		public ConvertStoredProcedureToDapperClass(ConvertSqlTypeToDotNetType getDotNetType) {
			this.getDotNetType = getDotNetType;
		}

		public Class Convert(Procedure procedure) {
			Class @class = new Class {
				Name = procedure.Name.Proper(),
				AccessModifier = AccessModifier.Public,
				Imports = new string[] { "Dapper", "System.Data", },
				Methods = new Method[] {
					GetCreateMethod(procedure),
				},
			};
			return @class;
		}

        object IConvertObject<Procedure>.Convert(Procedure from)
        {
            return this.Convert(from);
        }

        Method GetCreateMethod(Procedure procedure) {
			Method method = new Method("Create") {
				ReturnType = new DotNetType("Dapper.CommandDefinition"),
				Parameters = from sqlParam
							 in procedure.Parameters
							 select new Albatross.CodeGen.CSharp.Model.Parameter(Extension.VariableName(sqlParam.Name)) {
								 Type = getDotNetType.Convert(sqlParam.Type),
							 },
			};

			method.Body = new CodeBlock("DynamicParameters dynamicParameters = new DynamicParameters();\nreturn new CommandDefinition(dbConnection,);");
			return method;
		}
	}
}
