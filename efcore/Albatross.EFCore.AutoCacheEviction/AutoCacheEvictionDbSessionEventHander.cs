using Albatross.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Albatross.EFCore.AutoCacheEviction {
	public class AutoCacheEvictionDbSessionEventHander : IDbSessionEventHandler{
		private List<ICachedObject> changedEntities = new List<ICachedObject>();
		private readonly ICacheKeyManagement keyManagement;
		private readonly ILogger<AutoCacheEvictionDbSessionEventHander> logger;

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
			this.keyManagement.RemoveSelfAndChildren(this.changedEntities.ToArray());
			return Task.CompletedTask;
		}
	}
}
