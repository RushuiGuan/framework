using Albatross.Caching;
using Microsoft.EntityFrameworkCore;
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
		public Task PreSave(IDbSession session) => Task.CompletedTask;

		public Task PostSave() {
			if (cacheKeys.Any()) {
				var keys = this.keyManagement.RemoveSelfAndChildren(this.cacheKeys.ToArray());
				logger.LogInformation("Cache keys evicted:\n{@keys}", keys);
			}
			return Task.CompletedTask;
		}

		public void OnAddedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject<DbContext, PropertyEntry> cachedObject) {
				var newKeys = cachedObject.CreateCacheKeys(ObjectState.Added, entry.Context, Array.Empty<PropertyEntry>());
				cacheKeys.AddRange(newKeys);
			}
		}

		public void OnModifiedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject<DbContext, PropertyEntry> cachedObject) {
				var newKeys = cachedObject.CreateCacheKeys(ObjectState.Modified, entry.Context, entry.Properties.Where(x => x.IsModified).ToArray());
				cacheKeys.AddRange(newKeys);
			}
		}

		public void OnDeletedEntry(EntityEntry entry) {
			if (entry.Entity is ICachedObject<DbContext, PropertyEntry> cachedObject) {
				var newKeys = cachedObject.CreateCacheKeys(ObjectState.Deleted, entry.Context, entry.Properties.ToArray());
				cacheKeys.AddRange(newKeys);
			}
		}
	}
}