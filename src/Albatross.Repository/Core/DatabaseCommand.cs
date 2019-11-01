using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Repository.Core {
	public class DatabaseCommand {
		public DatabaseCommand(IDbSession dbSession) {
			this.DbSession = dbSession;
		}
		public IDbSession DbSession { get; private set; }

		protected T ExecuteScalar<T>(CommandDefinition commandDefinition) {
			return DbSession.DbConnection.ExecuteScalar<T>(commandDefinition);
		}
		protected object ExecuteScalar(CommandDefinition commandDefinition) {
			return DbSession.DbConnection.ExecuteScalar(commandDefinition);
		}
		protected void Execute(CommandDefinition commandDefinition) {
			DbSession.DbConnection.Execute(commandDefinition);
		}



		protected Task<T> ExecuteScalarAsync<T>(CommandDefinition commandDefinition) {
			return DbSession.DbConnection.ExecuteScalarAsync<T>(commandDefinition);
		}
		protected Task<object> ExecuteScalarAsync(CommandDefinition commandDefinition) {
			return DbSession.DbConnection.ExecuteScalarAsync(commandDefinition);
		}
		protected Task<int> ExecuteAsync(CommandDefinition commandDefinition) {
			return DbSession.DbConnection.ExecuteAsync(commandDefinition);
		}
	}
	public class DatabaseCommand<T> : DatabaseCommand {
		public DatabaseCommand(IDbSession dbSession) : base(dbSession) { }

		protected IEnumerable<T> Query(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.Query<T>(commandDefinition);
		}
		protected T QueryFirst(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.QueryFirst<T>(commandDefinition);
		}
		protected T QueryFirstOrDefault(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.QueryFirstOrDefault<T>(commandDefinition);
		}
		protected T QuerySingle(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.QuerySingle<T>(commandDefinition);
		}
		protected T QuerySingleOrDefault(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.QuerySingleOrDefault<T>(commandDefinition);
		}



		protected Task<IEnumerable<T>> QueryAsync(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.QueryAsync<T>(commandDefinition);
		}
		protected Task<T> QueryFirstAsync(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.QueryFirstAsync<T>(commandDefinition);
		}
		protected Task<T> QueryFirstOrDefaultAsync(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.QueryFirstOrDefaultAsync<T>(commandDefinition);
		}
		protected Task<T> QuerySingleAsync(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.QuerySingleAsync<T>(commandDefinition);
		}
		protected Task<T> QuerySingleOrDefaultAsync(CommandDefinition commandDefinition) {
			return this.DbSession.DbConnection.QuerySingleOrDefaultAsync<T>(commandDefinition);
		}
	}
}
