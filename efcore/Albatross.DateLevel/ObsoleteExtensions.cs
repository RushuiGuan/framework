#if NET6_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.DateLevel {
	public static class ObsoleteExtensions {
		/// <summary>
		/// Provided a date level series and effective date. This method will search and return the items
		/// where the given date falls within the date range.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="startDate"></param>
		/// <returns></returns>
		[Obsolete($"Use {nameof(DateLevelEntityExtensions.GetDateLevelEntitiesByDate)} instead")]
		public static IEnumerable<T> GetDateLevelEntityByDate<T>(this IEnumerable<T> source, DateOnly effectiveDate) where T : DateLevelEntity
			=> DateLevelEntityExtensions.GetDateLevelEntitiesByDate<T>(source, effectiveDate);

		[Obsolete($"Use {nameof(DateLevelEntityExtensions.GetDateLevelEntityByDate)} instead")]
		public static T? GetDateLevelItemByDate<T, K>(this IEnumerable<T> source, K key, DateOnly effectiveDate)
			where T : DateLevelEntity<K> where K : IEquatable<K> => DateLevelEntityExtensions.GetDateLevelEntityByDate<T, K>(source, key, effectiveDate);

		/// <summary>
		/// Provided a date level series and a date range, this method will search and return items
		/// where the date range falls between the start and end dates.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <returns></returns>
		[Obsolete($"Use {nameof(DateLevelEntityExtensions.GetOverlappedDateLevelEntities)} instead")]
		public static IEnumerable<T> GetDateLevelEntityByDateRange<T>(this IEnumerable<T> source, DateOnly start, DateOnly end)
			where T : DateLevelEntity {
			return source.Where(args => !(start > args.EndDate || end < args.StartDate));
		}
	}
}
#endif