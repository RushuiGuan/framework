using Albatross.Caching.BuiltIn;
using Microsoft.EntityFrameworkCore;
using Sample.EFCore.Models;

namespace Sample.EFCore {
	public class MyDataService {
		private readonly SampleDbSession session;
		private readonly OneDayCache<MyData, MyDataCacheKey> cache;

		public MyDataService(SampleDbSession session, OneDayCache<MyData, MyDataCacheKey> cache) {
			this.session = session;
			this.cache = cache;
		}
		public Task<MyData> Get(int id) {
			return cache.ExecuteAsync(async _ => await session.DbContext.Set<MyData>().Where(x => x.Id == id).FirstAsync(), new MyDataCacheKey(id));
		}

		public async Task Set(MyData data) {
			if (data.Id == 0) {
				session.DbContext.Set<MyData>().Add(data);
			} else {
				session.DbContext.Set<MyData>().Update(data);
			}
			await session.SaveChangesAsync();
		}
	}
}