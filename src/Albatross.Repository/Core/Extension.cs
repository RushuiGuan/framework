using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Repository.Core {
	public static class Extension {
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) {
			foreach (var item in items) {
				collection.Add(item);
			}
		}

		public static void ValidateByDataAnnotations(this object entity) {
			Validator.ValidateObject(entity, new ValidationContext(entity), true);
		}
	}
}