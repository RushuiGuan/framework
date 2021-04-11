using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Albatross.Authentication.Server {
	public interface IUserProfileService {
		Task<User> GetUser(string account);
		Task<IEnumerable<User>> Search();
	}

	#pragma warning disable CA1416 // Validate platform compatibility
	public class ActiveDirectoryUserProfileService : IUserProfileService {
		Lazy<PrincipalContext> lazyPrincipalContext;
		private readonly ILogger<ActiveDirectoryUserProfileService> logger;
		private readonly ActiveDirectoryConfig config;

		public ActiveDirectoryUserProfileService(ILogger<ActiveDirectoryUserProfileService> logger, ActiveDirectoryConfig config) {
			this.logger = logger;
			this.config = config;
			lazyPrincipalContext = new Lazy<PrincipalContext>(() => new PrincipalContext(ContextType.Domain, config.DomainName, config.LdapString));
							  //lazyPrincipalContext = new Lazy<PrincipalContext>(() => new PrincipalContext(ContextType.Domain, config.DomainName));
		}


		public Task<User> GetUser(string account) {
			return Task.Run<User>(() => {
				using (var userPrincipal = UserPrincipal.FindByIdentity(lazyPrincipalContext.Value, account)) {
					var user = CreateUser(userPrincipal);
					return user;
				}
			});
		}

		private User CreateUser(UserPrincipal userPrincipal) {
			logger.LogInformation("Getting user profile: {name}", userPrincipal.Name);
			User user = new User {
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

		public void Dispose() {
			if (this.lazyPrincipalContext.IsValueCreated) {
				this.lazyPrincipalContext.Value.Dispose();
			}
		}

		public Task<IEnumerable<User>> Search() {
			return Task.Run<IEnumerable<User>>(() => {
				UserPrincipal searchPrincipal = new UserPrincipal(this.lazyPrincipalContext.Value);
				searchPrincipal.Enabled = true;
				using (var searcher = new PrincipalSearcher()) {
					searcher.QueryFilter = searchPrincipal;
					List<User> list = new List<User>();
					foreach (var item in searcher.FindAll()) {
						if (item is UserPrincipal) {
							UserPrincipal userPrincipal = (UserPrincipal)item;
							if (userPrincipal.Enabled == true) {
								var user = CreateUser(userPrincipal);
								list.Add(user);
							}
						}
					}
					return list;
				}
			});
		}
	}
	#pragma warning restore CA1416 // Validate platform compatibility
}
