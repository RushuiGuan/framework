using Albatross.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Albatross.EFCore.AutoCacheEviction {
	public class AutoCacheEvictionDbSessionEventHander : IDbSessionEventHandler{
		private List<ICachedObject> changedEntities = new List<ICachedObject>();
		private readonly CacheEvictionService cacheEvictionService;
		private readonly ILogger<AutoCacheEvictionDbSessionEventHander> logger;

		public bool InvalidateAdded { get; set; }

		public AutoCacheEvictionDbSessionEventHander(CacheEvictionService cacheEvictionService, ILogger<AutoCacheEvictionDbSessionEventHander> logger) {
			this.cacheEvictionService = cacheEvictionService;
			this.logger = logger;
		}

		public void PriorSave(IDbSession session) {
			foreach(var entry in session.DbContext.ChangeTracker.Entries()) {
				if(entry is ICachedObject cachedObject) {
					if (entry.State == EntityState.Modified) {
						changedEntities.Add(cachedObject);
					}else if(entry.State == EntityState.Deleted) {
						changedEntities.Add(cachedObject);
					} else if (this.InvalidateAdded && entry.State == EntityState.Added) {
						changedEntities.Add(cachedObject);
					}
				}
			}
		}

		public Task PostSave() {
			foreach (var item in changedEntities) {
				try {
					item.Invalidate(this.cacheEvictionService);
				} catch (Exception err) {
					logger.LogError(err, "Error invalidating cache for {type}", item.GetType().Name);
				}
			}
			return Task.CompletedTask;
		}
	}
}
