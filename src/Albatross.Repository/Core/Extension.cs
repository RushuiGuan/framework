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

		//public static async Task SetDateLevel2<T>(this DateLevelEntity<T> entity, DbSet<DateLevelEntity<T>> set) where T : class {

		//	set.Where(args => args.StartDate <= start)


		//	var items = await src.GetChanged(set).ToArrayAsync();
		//	foreach (var item in items) {
		//		if (item.EndDate >= src.StartDate && item.EndDate <= src.StartDate) {
		//		} else if (item.StartDate >= src.StartDate && item.StartDate <= src.EndDate) {
		//		} else if (item.StartDate < src.StartDate && item.EndDate > src.EndDate) {
		//			if (!item.Entity.Equals(src.Entity)) {
		//				item.EndDate = src.StartDate.AddDays(-1);
		//			}
		//		}
		//	}
		//}

		/// <summary>
		/// when inserting a date level entry, we don't change the start date since it is part of the primary key.
		/// If this situation come up, we would delete the old entry and create a new one
		/// For DateLevel entries, there should be no gap between the first StartDate and <see cref="DateLevelEntity.MaxEndDate"/>
		/// Assuming all operations abide this rule, the method below will not check if the EndDate is correct but assume that it is the case
		/// </summary>
		public static async Task SetDateLevel<T>(this T src, DbSet<T> set) where T : DateLevelEntity {
			var after = await set.Where(args => args.Id == src.Id && args.StartDate >= src.StartDate)
				.OrderBy(args => args.StartDate)
				.FirstOrDefaultAsync();

			if (after == null) {
				src.EndDate = DateLevelEntity.MaxEndDate;
				set.Add(src);
			} else if (after.Equals(src)) {
				after.StartDate = src.StartDate;
				after.ModifiedBy = src.ModifiedBy;
				after.ModifiedUtc = DateTime.UtcNow;
				src = after;
			} else if (after.StartDate == src.StartDate) {
				after.Update(src);
				src = after;
			} else {
				src.EndDate = after.StartDate.AddDays(-1);
				set.Add(src);
			}

			var before = await set.Where(args => args.Id == src.Id && args.StartDate < src.StartDate)
				.OrderByDescending(args => args.StartDate)
				.FirstOrDefaultAsync();

			if (before != null) {
				if (before.Equals(src)) {
					before.EndDate = src.EndDate;
					before.ModifiedBy = src.ModifiedBy;
					before.ModifiedUtc = DateTime.UtcNow;
					set.Remove(src);
				} else {
					before.EndDate = src.StartDate.AddDays(-1);
					before.ModifiedBy = src.ModifiedBy;
					before.ModifiedUtc = DateTime.UtcNow;
				}
			}
		}
	}
}