using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

		//public static async Task SetDateLevelAsync<T, K>(this IQueryable<T> set, T src, Action<T> add, Action<T> remove, bool setMaxEndDate = false)
		//	where K : IEquatable<K>
		//	where T : DateLevelEntity<K> {

		//	if (setMaxEndDate) {
		//		var items = await set
		//			.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate)
		//			.ToArrayAsync();
		//		foreach (var item in items) {
		//			remove(item);
		//		}
		//		src.EndDate = DateLevelEntity.MaxEndDate;
		//		add(src);
		//	} else {
		//		var after = await set
		//			.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate)
		//			.OrderBy(args => args.StartDate)
		//			.FirstOrDefaultAsync();

		//		if (after == null) {
		//			src.EndDate = DateLevelEntity.MaxEndDate;
		//			add(src);
		//		} else {
		//			var changed = !after.HasSameValue(src);
		//			if (changed && after.StartDate != src.StartDate) {
		//				src.EndDate = after.StartDate.AddDays(-1);
		//				add(src);
		//			} else if (changed || after.StartDate != src.StartDate) {
		//				src.EndDate = after.EndDate;
		//				remove(after);
		//				add(src);
		//			}
		//		}
		//	}
		//	var before = await set.Where(args => args.Key.Equals(src.Key) && args.StartDate < src.StartDate)
		//		.OrderByDescending(args => args.StartDate)
		//		.FirstOrDefaultAsync();
		//	if (before != null) {
		//		if (before.HasSameValue(src)) {
		//			before.EndDate = src.EndDate;
		//			remove(src);
		//		} else {
		//			before.EndDate = src.StartDate.AddDays(-1);
		//		}
		//	}
		//}

		/// <summary>
		/// provided the data level collection for a single entity, this method will create a new entry for the series and adjust the end date for other items
		/// in the same entity if necessary
		/// 
		/// For DateLevel entries, two rules apply
		/// 1. there should be no gap between the first StartDate and <see cref="DateLevelEntity.MaxEndDate"/>
		/// 2. there should be no overlap of dates among entries.
		/// Assuming all operations abide these rules, the method below will not check if the EndDate is correct and simply assume that is the case
		/// this implementation is treating datelevel entity has immutable values and only its StartDate and EndDate can be changed
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <param name="collection"></param>
		/// <param name="src"></param>
		/// <param name="insert"></param>
		public static void SetDateLevel<T, K>(this ICollection<T> collection, T src, bool insert = false)
			where K : IEquatable<K>
			where T : DateLevelEntity<K> {

			if (insert) {
				var after = collection.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate)
					.OrderBy(args => args.StartDate).FirstOrDefault();

				if (after == null) {
					src.EndDate = DateLevelEntity.MaxEndDate;
					collection.Add(src);
				} else {
					var changed = !after.HasSameValue(src);
					if (changed && after.StartDate != src.StartDate) {
						src.EndDate = after.StartDate.AddDays(-1);
						collection.Add(src);
					} else if (changed || after.StartDate != src.StartDate) {
						src.EndDate = after.EndDate;
						collection.Remove(after);
						collection.Add(src);
					}
				}
			} else {
				var items = collection.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate).ToArray();
				bool hasExisting = false;
				foreach (var item in items) {
					if (item.StartDate == src.StartDate && item.HasSameValue(src)) {
						hasExisting = true;
						item.EndDate = DateLevelEntity.MaxEndDate;
					} else {
						collection.Remove(item);
					}
				}
				if (!hasExisting) {
					src.EndDate = DateLevelEntity.MaxEndDate;
					collection.Add(src);
				}
			}
			var before = collection.Where(args => args.Key.Equals( src.Key) && args.StartDate < src.StartDate)
				.OrderByDescending(args => args.StartDate).FirstOrDefault();
			if (before != null) {
				if (before.HasSameValue(src)) {
					before.EndDate = src.EndDate;
					collection.Remove(src);
				} else {
					before.EndDate = src.StartDate.AddDays(-1);
				}
			}
		}

		/// <summary>
		/// Provided a date level series data for a single entity, the method will remove the datelevel item with the specified startDate.
		/// The method will always extend the end date of the previous record if it exists.  The method will not move 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="set"></param>
		/// <param name="startDate"></param>
		/// <param name="remove"></param>
		public static void DeleteDateLevel<T>(this IEnumerable<T> set, DateTime startDate, Action<T> remove)
			where T : DateLevelEntity {
			var current = set.Where(args => args.StartDate == startDate).FirstOrDefault();
			if (current != null) {
				remove(current);
				var before = set.Where(args => args.StartDate < current.StartDate)
					.OrderByDescending(args => args.StartDate)
					.FirstOrDefault();
				if (before != null) {
					before.EndDate = current.EndDate;
				}
			}
		}
		/// <summary>
		/// Provided a date level series data for a single entity, the method will rebuild the end dates and remove items if necessary
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="remove"></param>
		/// <returns></returns>
		public static void RebuildDateLevelSeries<T>(this IEnumerable<T> source, Action<T> remove)
			where T : DateLevelEntity {

			var items = source.OrderBy(args => args.StartDate).ToArray();
			T current = null;
			foreach (var item in items.ToArray()) {
				if (current == null) {
					current = item;
				} else {
					if (current.HasSameValue(item)) {
						remove(item);
						current.EndDate = item.EndDate;
					} else {
						current.EndDate = item.StartDate.AddDays(-1);
						current = item;
					}
				}
			}
			if (current != null) {
				current.EndDate = DateLevelEntity.MaxEndDate;
			}
		}
	}
}