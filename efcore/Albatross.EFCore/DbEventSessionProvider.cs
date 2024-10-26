using Microsoft.Extensions.DependencyInjection;

namespace Albatross.EFCore {
	/// <summary>
	/// singleton factory class that creates a new instance of IDbEventSession
	/// </summary>
	public interface IDbEventSessionProvider {
		IDbEventSession Create();
	}
	public class DbEventSessionProvider : IDbEventSessionProvider {
		private readonly IServiceScopeFactory scopeFactory;

		public DbEventSessionProvider(IServiceScopeFactory scopeFactory) {
			this.scopeFactory = scopeFactory;
		}

		public IDbEventSession Create() {
			return new DbEventSession(scopeFactory.CreateScope());
		}
	}
}