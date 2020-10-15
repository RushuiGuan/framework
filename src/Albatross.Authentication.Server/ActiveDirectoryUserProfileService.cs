using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Albatross.Authentication.Server {
	public interface IUserProfileService {
		Task<User> GetUser(string account);
	}

	public class ActiveDirectoryUserProfileService : IUserProfileService {
		Lazy<PrincipalContext> lazyPrincipalContext = new Lazy<PrincipalContext>(() => new PrincipalContext(ContextType.Domain));
		private readonly ILogger<ActiveDirectoryUserProfileService> logger;
		
		public ActiveDirectoryUserProfileService(ILogger<ActiveDirectoryUserProfileService> logger) {
			this.logger = logger;
		}


		public Task<User> GetUser(string account) {
			return Task.Run<User>(() => {
				using (var userPrincipal = UserPrincipal.FindByIdentity(lazyPrincipalContext.Value, account)) {
					var user = new User {
						Name = userPrincipal.Name,
						Account = userPrincipal.SamAccountName,
						Email = userPrincipal.EmailAddress,
					};
					var groups = userPrincipal.GetGroups().Cast<GroupPrincipal>().Where(args => args.IsSecurityGroup == true);
					user.Groups = (from item in groups select new Group {
						Name = item.Name,
						Description = item.Description,
						Account = item.SamAccountName,
					}).ToArray();

					return user;
				}
			});
		}
		
		public void Dispose() {
			if (this.lazyPrincipalContext.IsValueCreated) {
				this.lazyPrincipalContext.Value.Dispose();
			}
		}
	}
}
