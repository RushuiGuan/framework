using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

		public static IQueryable<DateLevelEntity> GetChanged<T>(this DateLevelEntity entity, IQueryable<DateLevelEntity> set) where T : class {
			return set.Where(args => args.Id == entity.Id && (args.StartDate >= entity.StartDate && args.StartDate <= entity.EndDate
					|| args.EndDate >= entity.StartDate && args.EndDate <= entity.EndDate
					|| args.StartDate < entity.StartDate && args.EndDate > entity.EndDate
				));
		}

		/// <summary>
		/// For DateLevel entries, two rules apply
		/// 1. there should be no gap between the first StartDate and <see cref="DateLevelEntity.MaxEndDate"/>
		/// 2. there should be no overlap of dates among entries.
		/// Assuming all operations abide these rules, the method below will not check if the EndDate is correct and simply assume that is the case
		/// </summary>
		public static async Task SetDateLevel<T>(this T src, IQueryable<T> set, Action<T> add, Action<T> remove, string user, bool removePostDateEntries=false) where T : DateLevelEntity {
			var current = await set.Where(args => args.Id == src.Id && args.StartDate == src.StartDate)
				.FirstOrDefaultAsync();
			if (current != null) {
				current.Update(src);
				current.ModifiedBy = user;
				current.ModifiedUtc = DateTime.UtcNow;
			} else {
				if (removePostDateEntries) {
					var items = await set
						.Where(args => args.Id == src.Id && args.StartDate >= src.StartDate)
						.ToArrayAsync();
					foreach (var item in items) { 
						remove(item); 
					}
					src.EndDate = DateLevelEntity.MaxEndDate;
					add(src);
				} else {
					var after = await set
						.Where(args => args.Id == src.Id && args.StartDate >= src.StartDate)
						.OrderBy(args => args.StartDate)
						.FirstOrDefaultAsync();

					if (after == null) {
						src.EndDate = DateLevelEntity.MaxEndDate;
						add(src);
					} else if (after.HasSameValue(src)) {
						src.EndDate = after.EndDate;
						remove(after);
						add(src);
					} else if (after.StartDate == src.StartDate) {
						after.Update(src);
						after.ModifiedBy = user;
						after.ModifiedUtc = DateTime.UtcNow;
						src = after;
					} else {
						src.EndDate = after.StartDate.AddDays(-1);
						add(src);
					}
				}
				var before = await set.Where(args => args.Id == src.Id && args.StartDate < src.StartDate)
					.OrderByDescending(args => args.StartDate)
					.FirstOrDefaultAsync();
				if (before != null) {
					if (before.HasSameValue(src)) {
						before.EndDate = src.EndDate;
						before.ModifiedBy = user;
						before.ModifiedUtc = DateTime.UtcNow;
						remove(src);
					} else {
						before.EndDate = src.StartDate.AddDays(-1);
						before.ModifiedBy = user;
						before.ModifiedUtc = DateTime.UtcNow;
					}
				}
			}
		}

		public static async Task DeleteDateLevel<T>(this DateLevelKey key, IQueryable<T> set, Action<T> remove, string user) where T : DateLevelEntity {
			var current = await set
				.Where(args => args.Id == key.Id && args.StartDate == key.StartDate)
				.FirstOrDefaultAsync();
			if (current != null) {
				remove(current);
				var before = await set.Where(args => args.Id == current.Id && args.StartDate < current.StartDate)
					.OrderByDescending(args => args.StartDate)
					.FirstOrDefaultAsync();
				if (before != null) {
					before.EndDate = current.EndDate;
					before.ModifiedBy = user;
					before.ModifiedUtc = DateTime.UtcNow;
				}
			}
		}
		public static async Task RebuildDateLevelSeries<T>(this IQueryable<T> set, int id, Action<T> remove) where T : DateLevelEntity {
			var items = await set.Where(args => args.Id == id)
				.OrderBy(args=>args.StartDate)
				.ToArrayAsync();
			T current = null;
			foreach(var item in items.ToArray()) {
				if(current == null) {
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