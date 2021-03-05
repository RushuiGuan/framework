using Albatross.Database;
using System.Data;
using System.Data.SqlClient;

namespace Albatross.Database.SqlServer {
	/// <summary>
	/// This implementation of <see cref="Albatross.Database.IGetDbConnection"/> will create a <see cref="System.Data.SqlClient.SqlConnection"/> object when provided a <see cref="Albatross.Database.Database"/> object.
	/// </summary>
	public class GetDbConnection : IGetDbConnection {
		IGetConnectionString getConnectionString;

		public GetDbConnection(IGetConnectionString getConnectionString) {
			this.getConnectionString = getConnectionString;
		}

		public IDbConnection Get(Database database) {
			string connectionString = getConnectionString.Get(database);
			return new SqlConnection(connectionString);
		}
	}
}
