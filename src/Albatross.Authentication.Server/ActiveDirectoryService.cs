using System.DirectoryServices;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using msg = Albatross.IAM.Messages;
using Albatross.Mapping.Core;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Albatross.Authentication.Server {
	public interface IActiveDirectoryService {
		IEnumerable<msg.User> GetGroupMembers(params GroupPrincipal[] groupPrincipals);
		msg.Group GetGroup(string groupName);
	}

	public class ActiveDirectoryService : IActiveDirectoryService, IDisposable {
		Lazy<PrincipalContext> lazyPrincipalContext = new Lazy<PrincipalContext>(() => new PrincipalContext(ContextType.Domain));
		private readonly IMapper<GroupPrincipal, msg.Group> groupMapper;
		private readonly IMapper<UserPrincipal, msg.User> userMapper;
		private readonly ILogger<ActiveDirectoryService> logger;

		public void Dispose() {
			if (this.lazyPrincipalContext.IsValueCreated) {
				this.lazyPrincipalContext.Value.Dispose();
			}
		}

		public ActiveDirectoryService(IMapper<GroupPrincipal, msg.Group> groupMapper, IMapper<UserPrincipal, msg.User> userMapper, ILogger<ActiveDirectoryService> logger) {
			this.groupMapper = groupMapper;
			this.userMapper = userMapper;
			this.logger = logger;
		}

		public IEnumerable<msg.User> GetGroupMembers(params GroupPrincipal[] groupPrincipals) {
			HashSet<string> groupSet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
			HashSet<string> userSet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

			Stack<GroupPrincipal> stack = new Stack<GroupPrincipal>();
			foreach (var item in groupPrincipals) {
				if (!groupSet.Contains(item.SamAccountName)) {
					groupSet.Add(item.SamAccountName);
					stack.Push(item);
				}
			}
			while (stack.Count > 0) {
				GroupPrincipal current = stack.Pop();
				foreach (var member in current.Members) {
					if (member is UserPrincipal) {
						UserPrincipal userPrincipal = (UserPrincipal)member;
						var entry = (DirectoryEntry)userPrincipal.GetUnderlyingObject();
						var uac = (int)entry.Properties["useraccountcontrol"].Value;
						var isEnabled = !Convert.ToBoolean(uac & 2);
						if (isEnabled) {
							if (!userSet.Contains(userPrincipal.SamAccountName)) {
								userSet.Add(userPrincipal.SamAccountName);
								yield return userMapper.Map<UserPrincipal, msg.User>(userPrincipal);
							}
						}
					} else if (!groupSet.Contains(member.SamAccountName)) {
						groupSet.Add(member.SamAccountName);
						stack.Push((GroupPrincipal)member);
					}
				}
			}
		}

		public msg.Group GetGroup(string groupName) {
			var adGroup = GroupPrincipal.FindByIdentity(lazyPrincipalContext.Value, groupName);
			if (adGroup != null) {
				var dst = new msg.Group();
				groupMapper.Map(adGroup, dst);
				dst.Assignments = (from item in GetGroupMembers(adGroup) select new msg.GroupAssignment { User = item }).ToList();
				return dst;
			}
			throw new System.Exception($"Active directory Group ({groupName}) not found");
		}

		public IEnumerable<msg.User> Search(string domainName, string ldapString) {
			using (var context = new PrincipalContext(ContextType.Domain, domainName, ldapString)) {
				UserPrincipal searchPrincipal = new UserPrincipal(context);
				searchPrincipal.Enabled = true;
				using (var searcher = new PrincipalSearcher()) {
					searcher.QueryFilter = searchPrincipal;

					foreach (var item in searcher.FindAll()) {
						if (item is UserPrincipal) {
							UserPrincipal userPrincipal = (UserPrincipal)item;
							yield return new msg.User {
								Account = userPrincipal.SamAccountName,
								Name = userPrincipal.DisplayName,
								ProviderID = IAMConstants.PrincipalProvider.WindowsID,
								PrincipalType = PrincipalType.User.ToString(),
								Email = userPrincipal.EmailAddress,
								Active = true,
							};
						}
					}
				}
			}
		}
	}

	//public User GetUser(string account, User createdBy) {
	//	using (var context = new PrincipalContext(ContextType.Domain)) {
	//		var userPrincipal = UserPrincipal.FindByIdentity(context, account);
	//		if (createdBy != null) {
	//			return new User(userPrincipal, createdBy);
	//		} else {
	//			throw new System.Exception($"ActiveDirectory User ({account}) not found");
	//		}
	//	}
	//}

	//public Group GetGroup(string groupAccount, User createdBy) {
	//	using (var context = new PrincipalContext(ContextType.Domain)) {
	//		var adGroup = GroupPrincipal.FindByIdentity(context, groupAccount);
	//		if (adGroup != null) {
	//			var iamGroup = new Group(adGroup, createdBy);
	//			List<GroupAssignment> list = new List<GroupAssignment>();
	//			iamGroup.GroupAssignments = list;
	//			this.GetGroupMember(adGroup, list);
	//			return iamGroup;
	//		} else {
	//			throw new System.Exception($"ActiveDirectory Group ({groupAccount}) not found");
	//		}
	//	}
	//}

	///// <summary>
	///// Return all users under the active directory group recursively. This method will process each group recursive exactly once even when it is referenced multiple times in the tree.
	///// </summary>
	///// <param name="groups">a list of active directory groups.</param>
	///// <param name="createdBy"></param>
	///// <returns></returns>
	//private IEnumerable<User> GetGroupUsers(IEnumerable<string> groupAccounts, User createdBy) {
	//	HashSet<string> groupSet = new HashSet<string>(groupAccounts, StringComparer.InvariantCultureIgnoreCase);
	//	HashSet<string> userSet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

	//	using (var context = new PrincipalContext(ContextType.Domain)) {
	//		Stack<GroupPrincipal> stack = new Stack<GroupPrincipal>();
	//		foreach (string groupName in groupAccounts) {
	//			using (var group = GroupPrincipal.FindByIdentity(context, groupName)) {
	//				if (group != null) {
	//					stack.Push(group);
	//				}
	//			}
	//		}

	//		while (stack.Count > 0) {
	//			GroupPrincipal current = stack.Pop();
	//			foreach (var member in current.Members) {
	//				if (member is UserPrincipal) {
	//					if (!userSet.Contains(member.SamAccountName)) {
	//						userSet.Add(member.SamAccountName);
	//						yield return new User((UserPrincipal)member, createdBy);
	//					}
	//				} else if (!groupSet.Contains(member.SamAccountName)) {
	//					groupSet.Add(member.SamAccountName);
	//					stack.Push((GroupPrincipal)member);
	//				}
	//			}
	//		}
	//	}
	//}



	//public async Task SyncAllUsers(User current) {
	//	using (var context = new PrincipalContext(ContextType.Domain)) {
	//		var foundGroups = (
	//			from item in groups.Items where item.Active && item.ProviderID == IAMConstants.PrincipalProvider.WindowsID
	//			select GroupPrincipal.FindByIdentity(context, item.Account)
	//		).Where(args => args != null).ToArray();
	//		var foundUsers = GetGroupMembers(foundGroups);
	//		var windowsUsers = (from item in users.Items where item.ProviderID == IAMConstants.PrincipalProvider.WindowsID select item).ToArray();
	//		windowsUsers.Merge<UserPrincipal, User, string>(
	//			foundUsers,
	//			args => args.Account, args => args.SamAccountName,
	//			(src, dst) => dst.Update(new User(src, current), current),
	//			src => users.Add(new User(src, current)),
	//			dst => { dst.Active = false; }
	//		);
	//	}
	//	await users.SaveChangesAsync();
	//}

	//public async Task SyncGroups(params string[] groupNames, User current, bool throwIfNotFound) {
	//	using (var context = new PrincipalContext(ContextType.Domain)) {
	//		List<Group> list = new List<Group>();
	//		foreach (string groupName in groupNames) {
	//			var adGroup = GroupPrincipal.FindByIdentity(context, groupName);
	//			if (adGroup != null) {
	//				var iamGroup = new Group(adGroup, current);
	//				list.Add(iamGroup);
	//				iamGroup.GroupAssignments = list;
	//				this.GetGroupMember(adGroup, list);
	//				return iamGroup;
	//			} else if (throwIfNotFound) {
	//				throw new System.Exception($"ActiveDirectory Group ({groupName}) not found");
	//			}
	//		}
	//	}
	//}

	
}
