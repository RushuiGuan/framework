using System;
using System.Reflection;

namespace Albatross.Messaging.Core {
	public static class Extensions {
		public static string GetCommandName(this Type type) => type.GetCustomAttribute<CommandNameAttribute>(false)?.Name
				?? type.FullName ?? throw new InvalidOperationException($"Command type {type.Name} does not have a full name");
	}
}
