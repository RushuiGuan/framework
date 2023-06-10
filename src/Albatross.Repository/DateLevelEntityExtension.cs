using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Albatross.Repository {
	public static class DateLevelEntityExtensions {
		/// <summary>
		/// /// For DateLevel entries, two rules apply
		/// 1. there should be no gap between the first StartDate and <see cref="DateLevelEntity.MaxEndDate"/>
		/// 2. there should be no overlap of dates among entries.
		/// Assuming all operations abide these rules, the method below will not check if the EndDate is correct and simply assume that is the case
		/// this implementation is treating datelevel entity has immutable values and only its StartDate and EndDate can be changed
		/// 
		/// Provided the data level collection for a single entity, this method will create a new entry for the series and adjust the end date for other items within the the same entity 
		/// if necessary.  There are 3 possible operations.
		/// 1. insert operation with start date only: create a record in the middle of the time series with only a start date.  End date is determined automatically.
		/// 2. insert operation with both start date and end date: create a record in the middle of time series.  Adjust the existing overrlapping entries accordingly.
		/// 3. append operation with only start date: create an entry with a start date and max end date and remove any current overlapping entries.
		///
		/// If the insert flag is false, the method will always append the record by setting the end date as the <see cref="DateLevelEntity.MaxEndDate"/>
		/// The method will remove any existing record between the start date and the end date
		/// 
		/// If the insert flag is true and the end date of the record equals <see cref="DateLevelEntity.MaxEndDate"/>, the method will insert using the start date only.  If the end date of the
		/// record is specified, the method will insert only if it 
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
				if (src.EndDate == DateLevelEntity.MaxEndDate) {
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
					// throw new NotSupportedException("Insert date level entity with an end date is not yet supported");
					var after = collection
						.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate && args.StartDate <= src.EndDate).ToArray();
					if (after.Length == 0) {
						throw new InvalidOperationException($"Cannot insert date level item at the end of time series or within a single date level entry");
					} else {
						foreach (var item in after) {
							if (item.EndDate <= src.EndDate) {
								collection.Remove(item);
							} else {
								var changed = !item.HasSameValue(src);
								if (changed) {
									// make a clone of the item, remove and insert the new.  This is
									// due to the start date being part of the key
									T newItem = (T)item.Clone();
									newItem.StartDate = src.EndDate.AddDays(1);
									collection.Remove(item);
									collection.Add(newItem);
								} else {
									src.EndDate = item.EndDate;
									collection.Remove(item);
								}
								collection.Add(src);
							}
						}
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
			var before = collection.Where(args => args.Key.Equals(src.Key) && args.StartDate < src.StartDate)
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

		/// <summary>
		/// Provided a date level series and effective date. This method will search and return the items
		/// where the given date falls within the date range.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="startDate"></param>
		/// <returns></returns>
		public static IEnumerable<T> GetDateLevelEntityByDate<T>(this IEnumerable<T> source, DateTime effectiveDate)
			where T : DateLevelEntity {
			var items = source.Where(args => args.StartDate <= effectiveDate && args.EndDate >= effectiveDate).ToArray();
			return items;
		}

		public static T? GetDateLevelItemByDate<T, K>(this IEnumerable<T> source, K key, DateTime effectiveDate)
			where T : DateLevelEntity<K> where K : IEquatable<K> {
			var item = source.Where(args => args.Key.Equals(key)
				&& args.StartDate <= effectiveDate
				&& args.EndDate >= effectiveDate).FirstOrDefault();
			return item;
		}
	}
}