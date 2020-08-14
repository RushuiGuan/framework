using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace Albatross.Azure {
	[Route("api/user-profile")]
	[ApiController]
	[Authorize]
	public class UserProfileController : ControllerBase {
		public const string ScopeUserRead = "User.Read";
		public const string ScopeDirectoryReadAll = "Directory.Read.All";
		public readonly static string[] GraphApiScopes = new string[] {
			ScopeUserRead, ScopeDirectoryReadAll,
		};

		private readonly ITokenAcquisition tokenAcquisition;
		private readonly IGraphServiceClientFactory graphServiceClientFactory;

		public UserProfileController(ITokenAcquisition tokenAcquisition, IGraphServiceClientFactory graphServiceClientFactory) {
			this.tokenAcquisition = tokenAcquisition;
			this.graphServiceClientFactory = graphServiceClientFactory;
		}

		public class MyUser { 
			public string Name { get; set; }
		}

		[HttpGet("profile")]
		public async Task<MyUser> Profile() {
			var client = graphServiceClientFactory.Create(this.tokenAcquisition, GraphApiScopes);
			User user = await client.Me.Request().GetAsync();
			return new MyUser {
				Name = user.DisplayName,
			};
		}
	}
}
