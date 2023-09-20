using Albatross.Caching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Hosting {
	[Route("api/caching")]
	[ApiController]
	[Authorize]
	public class CachingController : ControllerBase {
		private readonly ICacheKeyManagement keyMgmt;
		private readonly ICacheProviderAdapter cacheProviderConverter;

		public CachingController(ICacheKeyManagement keyMgmt, ICacheProviderAdapter cacheProviderConverter) {
			this.keyMgmt = keyMgmt;
			this.cacheProviderConverter = cacheProviderConverter;
		}

		[HttpGet("keys")]
		public IEnumerable<string> Keys() => keyMgmt.FindKeys("*");

		[HttpGet]
		public async Task<string?> Get(string key) {
			var provider = cacheProviderConverter.Create<string>();
			var result = await provider.TryGetAsync(key, CancellationToken.None, false);
			if (result.Item1) {
				return result.Item2;
			} else {
				return null;
			}
		}

		[HttpPost("remove")]
		public void Remove([FromBody] IEnumerable<string> keys) {
			this.keyMgmt.Remove(keys.ToArray());
		}

		[HttpPost("reset")]
		public void Reset() {
			this.keyMgmt.FindAndRemoveKeys("*");
		}
	}
}
