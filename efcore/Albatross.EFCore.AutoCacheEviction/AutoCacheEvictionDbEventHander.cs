using Albatross.Caching;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Albatross.EFCore.AutoCacheEviction {
	public class AutoCacheEvictionDbEventHander : IDbSessionEventHandler {
		private List<ICacheKey> cacheKeys = new List<ICacheKey>();
		private readonly ICacheKeyManagement keyManagement;
		private readonly ILogger<AutoCacheEvictionDbEventHander> logger;

		public AutoCacheEvictionDbEventHander(ICacheKeyManagement keyManagement, ILogger<AutoCacheEvictionDbEventHander> logger) {
			this.keyManagement = keyManagement;
			this.logger = logger;
		}
		public void PreSave(IDbSession session) { }

		public Task PostSave() {
			if (cacheKeys.Any()) {
				this.keyManagement.RemoveSelfAndChildren(this.cacheKeys.ToArray());
			}
			return Task.CompletedTask;
		}

		public void OnAddedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject cachedObject) {
				cacheKeys.Add(cachedObject.CreateCacheKey(ObjectState.Added, entry.OriginalValues));
			}
		}

		public void OnModifiedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject cachedObject) {
				cacheKeys.Add(cachedObject.CreateCacheKey(ObjectState.Modified, entry.OriginalValues));
			}
		}

		public void OnDeletedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject cachedObject) {
				cacheKeys.Add(cachedObject.CreateCacheKey(ObjectState.Deleted, entry.OriginalValues));
			}
		}
	}
}
