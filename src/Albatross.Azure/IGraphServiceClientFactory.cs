using Microsoft.Graph;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Albatross.Azure {
	/// <summary>
	/// this is needed because tokenAcquisition.GetAccessTokenForUserAsync can only be called after authentication.  Having it as part of the DI will invoke GetAccessTokenForUserAsync too early.
	/// </summary>
	public interface IGraphServiceClientFactory {
		IGraphServiceClient Create(ITokenAcquisition tokenAcquisition, params string[] scopes);
	}

	public class GraphServiceClientFactory : IGraphServiceClientFactory {
		public IGraphServiceClient Create(ITokenAcquisition tokenAcquisition, params string[] scopes) {
			DelegateAuthenticationProvider provider = new DelegateAuthenticationProvider(async (request) => {
				string token = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
				request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
			});
			return new GraphServiceClient(provider);
		}
	}
}
