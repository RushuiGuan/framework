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

		public void PriorSave(IDbSession session) {
			foreach(var entry in session.DbContext.ChangeTracker.Entries()) {
				if(entry is ICachedObject cachedObject) {
					if (entry.State == EntityState.Modified) {
						changedEntities.Add(cachedObject);
					}else if(entry.State == EntityState.Deleted) {
						changedEntities.Add(cachedObject);
					} else if (entry.State == EntityState.Added) {
						changedEntities.Add(cachedObject);
					}
				}
			}
		}

		public Task PostSave() {
			logger.LogInformation("Auto removing cache entry: ", this.changedEntities.SelectMany(x=>x.CacheKeys).Select(x=>x.WildCardKey).ToArray());
			this.keyManagement.RemoveSelfAndChildren(this.changedEntities.ToArray());
			return Task.CompletedTask;
		}

		public void OnAddedEntry(EntityEntry entry) {
			throw new NotImplementedException();
		}

		public void OnModifiedEntry(EntityEntry entry) {
			throw new NotImplementedException();
		}

		public void OnDeletedEntry(EntityEntry entry) {
			throw new NotImplementedException();
		}
	}
}
