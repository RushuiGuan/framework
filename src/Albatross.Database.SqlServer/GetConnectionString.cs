using Albatross.Database;
using System.Data;
using System.Data.SqlClient;

namespace Albatross.Database.SqlServer {
	/// <summary>
	/// The class uses <see cref="System.Data.SqlClient.SqlConnectionStringBuilder"/> to create a sql server connection string, unless the <see cref="Albatross.Database.Database.ConnectionString"/> property is specified
	/// </summary>
	public class GetConnectionString : IGetConnectionString {
		public string Get(Database db) {
			if (db == null) { return null; }
			SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
			if (string.IsNullOrEmpty(db.ConnectionString)) {
				sb.InitialCatalog = db.InitialCatalog;
				sb.DataSource = db.DataSource;
				if (db.SSPI) {
					sb.IntegratedSecurity = true;
				} else {
					sb.UserID = db.UserName;
					sb.Password = db.Password;
				}
				return sb.ToString();
			} else {
				return db.ConnectionString;
			}
		}
	}
}
