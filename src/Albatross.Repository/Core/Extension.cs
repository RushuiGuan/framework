using Albatross.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

		/// <summary>
		/// For DateLevel entries, two rules apply
		/// 1. there should be no gap between the first StartDate and <see cref="DateLevelEntity.MaxEndDate"/>
		/// 2. there should be no overlap of dates among entries.
		/// Assuming all operations abide these rules, the method below will not check if the EndDate is correct and simply assume that is the case
		/// this implementation is treating datelevel entity has immutable values and only its StartDate and EndDate can be changed
		/// </summary>
		public static async Task SetDateLevel<T, K>(this IQueryable<T> set, T src, Action<T> add, Action<T> remove, bool removePostDateEntries = false)
			where K : IEquatable<K>
			where T : DateLevelEntity<K> {

			if (removePostDateEntries) {
				var items = await set
					.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate)
					.ToArrayAsync();
				foreach (var item in items) {
					remove(item);
				}
				src.EndDate = DateLevelEntity.MaxEndDate;
				add(src);
			} else {
				var after = await set
					.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate)
					.OrderBy(args => args.StartDate)
					.FirstOrDefaultAsync();

				if (after == null) {
					src.EndDate = DateLevelEntity.MaxEndDate;
					add(src);
				} else {
					var changed = !after.HasSameValue(src);
					if (changed && after.StartDate != src.StartDate) {
						src.EndDate = after.StartDate.AddDays(-1);
						add(src);
					} else if (changed || after.StartDate != src.StartDate) {
						src.EndDate = after.EndDate;
						remove(after);
						add(src);
					}
				}
			}
			var before = await set.Where(args => args.Key.Equals(src.Key) && args.StartDate < src.StartDate)
				.OrderByDescending(args => args.StartDate)
				.FirstOrDefaultAsync();
			if (before != null) {
				if (before.HasSameValue(src)) {
					before.EndDate = src.EndDate;
					remove(src);
				} else {
					before.EndDate = src.StartDate.AddDays(-1);
				}
			}
		}

		// TODO: need unit test
		public static async Task DeleteDateLevel<T, K>(this IQueryable<T> set, K key, DateTime startDate, Action<T> remove)
			where K : IEquatable<K>
			where T : DateLevelEntity<K> {
			var current = await set
				.Where(args => args.Key.Equals(key) && args.StartDate == startDate)
				.FirstOrDefaultAsync();
			if (current != null) {
				remove(current);
				var before = await set.Where(args => args.Key.Equals(current.Key) && args.StartDate < current.StartDate)
					.OrderByDescending(args => args.StartDate)
					.FirstOrDefaultAsync();
				if (before != null) {
					before.EndDate = current.EndDate;
				}
			}
		}
		public static async Task RebuildDateLevelSeries<T, K>(this IQueryable<T> set, K key, Action<T> remove)
			where K : IEquatable<K>
			where T : DateLevelEntity<K> {
			var items = await set.Where(args => args.Key.Equals(key))
				.OrderBy(args => args.StartDate)
				.ToArrayAsync();
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