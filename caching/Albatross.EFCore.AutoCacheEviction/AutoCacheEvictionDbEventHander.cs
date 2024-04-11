using Albatross.Caching;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.EFCore.AutoCacheEviction {
	public class AutoCacheEvictionDbEventHander : IDbSessionEventHandler {
		private List<ICacheKey> cacheKeys = new List<ICacheKey>();
		private readonly ICacheKeyManagement keyManagement;
		private readonly ILogger<AutoCacheEvictionDbEventHander> logger;
		public override string ToString() => nameof(AutoCacheEvictionDbEventHander);

		public AutoCacheEvictionDbEventHander(ICacheKeyManagement keyManagement, ILogger<AutoCacheEvictionDbEventHander> logger) {
			this.keyManagement = keyManagement;
			this.logger = logger;
		}
		public void PreSave(IDbSession session) { }

		public Task PostSave() {
			if (cacheKeys.Any()) {
				var keys = this.keyManagement.RemoveSelfAndChildren(this.cacheKeys.ToArray());
				logger.LogInformation("Cache keys evicted:\n{@keys}", keys);
			}
			return Task.CompletedTask;
		}

		public void OnAddedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject<EntityEntry, PropertyEntry> cachedObject) {
				cacheKeys.AddRange(cachedObject.CreateCacheKeys(ObjectState.Added, entry, Array.Empty<PropertyEntry>()));
			}
		}

		public void OnModifiedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject<EntityEntry, PropertyEntry> cachedObject) {
				cacheKeys.AddRange(cachedObject.CreateCacheKeys(ObjectState.Modified, entry, entry.Properties.Where(x=>x.IsModified).ToArray()));
			}
		}

		public void OnDeletedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject<EntityEntry, PropertyEntry> cachedObject) {
				cacheKeys.AddRange(cachedObject.CreateCacheKeys(ObjectState.Deleted, entry, entry.Properties.ToArray()));
			}
		}
	}
}
