using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Database.SqlServer
{
	public static class Extension
	{
		public static IServiceCollection AddAlbatrossSqlDatabase(this IServiceCollection services) {
			services.AddSingleton<IGetConnectionString, GetConnectionString>();
			services.AddSingleton<IGetDbConnection, GetDbConnection>();
			services.AddSingleton<IGetProcedure, GetProcedure>();
			services.AddSingleton<IGetSqlType, GetSqlType>();
			services.AddSingleton<IGetTable, GetTable>();
			services.AddSingleton<IGetView, GetView>();
			services.AddSingleton<IGetTableColumnType, GetTableColumnType>();
			services.AddSingleton<IListProcedureParameter, ListProcedureParameter>();

			services.AddSingleton<IListSqlType, ListSqlType>();
			services.AddSingleton<IListTableColumn, ListTableColumn>();
			services.AddSingleton<IListTableIndex, ListTableIndex>();
			services.AddSingleton<IListTableIndexColumn, ListTableIndexColumn>();
			services.AddSingleton<IListTable, ListTable>();
			services.AddSingleton<IListProcedure, ListProcedure>();
			services.AddSingleton<IParseCriteria, ParseCriteria>();

			services.AddSingleton<ICheckProcedureCreated, CheckProcedureCreated>();
			services.AddSingleton<IDeployProcedure, DeployProcedure>();
			services.AddSingleton<IGetProcedureDefinition, GetProcedureDefinition>();
			services.AddSingleton<IGetDatabasePermission, GetDatabasePermission>();

			return services;
		}
	}
}
