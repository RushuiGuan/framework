using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class ListProcedureParameter : IListProcedureParameter {
		IGetDbConnection getDbConnection;

		public ListProcedureParameter(IGetDbConnection getDbConnection) {
			this.getDbConnection = getDbConnection;
		}

		public IEnumerable<Parameter> List(Procedure procedure) {
			using (var db = getDbConnection.Get(procedure.Database)) {
				return db.Query<Parameter, SqlType, Parameter>(@sql, (param, sqltype) => { param.Type = sqltype; return param; }, new { name = procedure.Name, schema = procedure.Schema,  }, splitOn: "Schema");
			}
		}


		string @sql = @"
select
	PARAMETER_NAME as Name,
	PARAMETER_MODE as Mode,
	cast(case IS_RESULT when 'no' then 0 else 1 end as bit) as IsResult,
	ORDINAL_POSITION as OrdinalPosition,

	isnull(USER_DEFINED_TYPE_SCHEMA, 'sys') as [Schema],
	case DATA_TYPE when 'table type' then User_Defined_Type_Name else Data_Type end as [Name],
	CHARACTER_MAXIMUM_LENGTH as MaxLength,
	ISNULL(numeric_precision, datetime_precision) as Precision,
	Numeric_scale as Scale,
	1 as IsNullable,
	cast (case when USER_DEFINED_TYPE_NAME is not null then 1 else 0 end as bit) as IsUserDefined,
	cast(case data_type when 'table type' then 1 else 0 end as bit) as IsTableType
from INFORMATION_SCHEMA.PARAMETERS 
where SPECIFIC_NAME = @name AND SPECIFIC_SCHEMA = @schema
";
	}
}
