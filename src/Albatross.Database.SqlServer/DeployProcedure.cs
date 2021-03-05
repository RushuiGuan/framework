using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class DeployProcedure : IDeployProcedure {
		IGetDbConnection getDbConnection;
		ICheckProcedureCreated checkProcedureCreated;
		public DeployProcedure(IGetDbConnection getDbConnection, ICheckProcedureCreated checkProcedureCreated) {
			this.getDbConnection = getDbConnection;
			this.checkProcedureCreated = checkProcedureCreated;
		}

		public void Deploy(Procedure procedure) {
			bool created = checkProcedureCreated.Check(procedure);
			using (var db = getDbConnection.Get(procedure.Database)) {
				if (created) {
					if (string.IsNullOrEmpty(procedure.AlterScript)) {
						throw new Exception("Alter script not found");
					} else {
						db.Execute(procedure.AlterScript);
					}
				} else {
					if (string.IsNullOrEmpty(procedure.CreateScript)) {
						throw new Exception("Create script not found");
					} else {
						db.Execute(procedure.CreateScript);
					}
				}
				if (!string.IsNullOrEmpty(procedure.PermissionScript)) {
					db.Execute(procedure.PermissionScript);
				}
			}
		}
	}
}
