using Albatross.Authentication.Core;
using Albatross.Caching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using System.Threading.Tasks;

namespace Albatross.Authentication.Server {
	[Route("api/user-profile")]
	[Authorize]
	public class UserProfileController : ControllerBase {
		private readonly IGetCurrentUser getCurrentUser;
		private readonly IUserProfileService userProfileService;
		private readonly ILogger<UserProfileController> logger;
		private readonly ICacheManagementFactory cacheMgmtFactory;
		private readonly IReadOnlyPolicyRegistry<string> cachePolicyRegistry;

		public UserProfileController(IGetCurrentUser getCurrentUser, IUserProfileService userProfileService,
			ILogger<UserProfileController> logger,
			ICacheManagementFactory cacheMgmtFactory,
			IReadOnlyPolicyRegistry<string> cachePolicyRegistry) {

			this.getCurrentUser = getCurrentUser;
			this.userProfileService = userProfileService;
			this.logger = logger;
			this.cacheMgmtFactory = cacheMgmtFactory;
			this.cachePolicyRegistry = cachePolicyRegistry;
		}



		[HttpGet]
		public async Task<User> Get() {
			string account = getCurrentUser.Get();
			var policy = cachePolicyRegistry.GetAsyncPolicy<User>(ActiveDirectoryUserProfileCacheMgmt.CacheKey);
			return await policy.ExecuteAsync(context => userProfileService.GetUser(account), new Context(account));
		}

		[HttpPost("{account}")]
		public void InvalidateCache(string account) {
			cacheMgmtFactory.GetCacheManagement(nameof(ActiveDirectoryUserProfileCacheMgmt)).Evict(new Polly.Context(account));
		}
	}
}
