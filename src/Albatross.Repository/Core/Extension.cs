using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Repository.Core {
	public static class Extension {
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) {
			foreach (var item in items) {
				collection.Add(item);
			}
		}

		public static Task<int> SaveChangesAsync<T>(this IRepository<T> repo, CancellationToken cancellationToken = default) {
			return repo.DbSession.SaveChangesAsync(cancellationToken);
		}
	}
}