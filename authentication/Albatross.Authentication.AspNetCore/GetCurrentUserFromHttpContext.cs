using Microsoft.AspNetCore.Http;

namespace Albatross.Authentication.AspNetCore {
	public class GetCurrentUserFromHttpContext : IGetCurrentUser {
		IHttpContextAccessor httpContextAccessor;
		public GetCurrentUserFromHttpContext(IHttpContextAccessor httpContextAccessor) {
			this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}
		public string Provider => "httpcontext";
		public string Get() => httpContextAccessor.HttpContext.GetIdentity();
	}
}