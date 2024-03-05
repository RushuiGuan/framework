using Albatross.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Albatross.EFCore.AutoCacheEviction {
	public class AutoCacheEvictionDbSessionEventHander : IDbChangeEventHandler{
		private List<ICachedObject> changedEntities = new List<ICachedObject>();
		private readonly ICacheKeyManagement keyManagement;
		private readonly ILogger<AutoCacheEvictionDbSessionEventHander> logger;

		public bool HasPostSaveOperation => changedEntities.Any();

		public AutoCacheEvictionDbSessionEventHander(ICacheKeyManagement keyManagement, ILogger<AutoCacheEvictionDbSessionEventHander> logger) {
			this.keyManagement = keyManagement;
			this.logger = logger;
		}

		public Task PostSave() {
			logger.LogInformation("Auto removing cache entry: ", this.changedEntities.SelectMany(x=>x.CacheKeys).Select(x=>x.WildCardKey).ToArray());
			this.keyManagement.RemoveSelfAndChildren(this.changedEntities.ToArray());
			return Task.CompletedTask;
		}

		public void OnAddedEntry(EntityEntry entry) {
			if(entry.Entity is ICachedObject cachedObject) {
				changedEntities.Add(cachedObject);
			}
		}

		public void OnModifiedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject cachedObject) {
				changedEntities.Add(cachedObject);
			}
		}

		public void OnDeletedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject cachedObject) {
				changedEntities.Add(cachedObject);
			}
		}
	}
}
