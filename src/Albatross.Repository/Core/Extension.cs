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

		// when inserting a date level entry, we don't change the start date since it is part of the primary key.
		// If this situation come up, we would delete the old entry and create a new one
		//public static async Task SetDateLevel1(this DateLevelEntity src, IQueryable<DateLevelEntity> set,
		//		Action<DateLevelEntity> add,
		//		Action<DateLevelEntity> remove) where T : class {

		//	var prior = await set.Where(args => args.Id == src.Id && args.StartDate <= src.StartDate)
		//		.OrderByDescending(args => args.StartDate)
		//		.FirstOrDefaultAsync();

		//	if (prior != null) {
		//		if (prior != src) {
		//			prior.ModifiedUtc = DateTime.UtcNow;
		//			prior.ModifiedBy = src.CreatedBy;
		//			if (prior.StartDate == src.StartDate) {
		//				prior.Entity = src.Entity;
		//				src = prior;
		//			} else {
		//				prior.EndDate = src.StartDate.AddDays(-1);
		//				add(src);
		//			}
		//		} else {
		//			return;
		//		}
		//	} else {
		//		add(src);
		//	}

		//	var post = await set.Where(args => args.Id == src.Id && args.StartDate > src.StartDate)
		//		.OrderBy(args => args.StartDate).FirstOrDefaultAsync();
		//	if (post != null) {
		//		if (post.Entity == src.Entity) {
		//			remove(post);
		//			src.EndDate = post.EndDate;
		//		} else {
		//			src.EndDate = post.StartDate.AddDays(-1);
		//		}
		//	} else {
		//		src.EndDate = DateLevelEntity<T>.MaxEndDate;
		//	}
		//}
	}
}