#if NET6_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Albatross.DateLevel {
	public static class DateLevelEntityExtensions {
		/// <summary>
		/// For DateLevel entries, two rules apply
		/// 1. there should be no gap between the first StartDate and <see cref="DateLevelEntity.MaxEndDate"/>
		/// 2. there should be no overlap of dates among entries.

		/// Provided the data level collection for a single entity, this method will create a new entry for the series and adjust the end date for other items within the the same entity 
		/// if necessary.  There are 3 possible operations.
		/// 1. insert operation with start date only: create a record in the middle of the time series with only a start date.  End date is determined automatically.
		/// 2. insert operation with both start date and end date: create a record in the middle of time series.  Adjust the existing overrlapping entries accordingly.
		/// 3. append operation with only start date: create an entry with a start date and max end date and remove any current overlapping entries.
		///
		/// If the insert flag is false, the method will always append the record by setting the end date as the <see cref="DateLevelEntity.MaxEndDate"/>
		/// The method will remove any existing record between the start date and the end date
		/// 
		/// If the insert flag is true and the end date of the record equals <see cref="DateLevelEntity.MaxEndDate"/>, the method will insert using the start date only.  
		/// If the end date of the record is specified, the method will insert only if the end date is one day before the start date of the next record
		/// 
		/// If a parent entity is used, the DeleteBehavior between the DateLevelEntity and the parent entity should be DeleteBehavior.ClientCascade or DeleteBehavior.Cascade
		/// This is because of the mismatch of behavior EFCore Cascade delete and sql server cascade delete.  <see href="https://github.com/dotnet/efcore/issues/10066"/>
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
				// when insert flag is true and EndDate is MaxEndDate, the code below will try to auto determine the correct end date of the new record
				if (src.EndDate == DateLevelEntity.MaxEndDate) {
					// assuming that date leve series are correct, find the next record of the series
					var after = collection.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate)
						.OrderBy(args => args.StartDate).FirstOrDefault();
					// if not found, the new record will be inserted as the last record and its end date will be set to max
					if (after == null) {
						src.EndDate = DateLevelEntity.MaxEndDate;
						collection.Add(src);
					} else {
						var changed = !after.HasSameValue(src);
						if (changed) {
							// if the next record is diff but they have the same start date, remove it and add the new record.  remember to set the end date of
							// the new record to the end date of the next record
							if (after.StartDate == src.StartDate) {
								src.EndDate = after.EndDate;
								collection.Remove(after);
								collection.Add(src);
							} else {
								// if the start date is diff,  set the end date of the new record to the day before the start date of the next record
								// add the new record
								src.EndDate = after.StartDate.AddDays(-1);
								collection.Add(src);
							}
						} else {
							// if the next record has the same value, simply change the start date of the next record
							// remember to set next record as the new record
							after.StartDate = src.StartDate;
							src = after;
						}
					}
				} else {
					// code path here is for inserting a new record with the specified start date and end date.
					// it assumes that the series is built correctly
					// if the collection is empty, we cannot insert a new record with a specific end date, because it will break rule #1 and create an incorrect series.
					// Also, inserting a new record within the start date and end date of an existing record is an impossible operation that cannot be addressed
					// in this code path. Therefore an exception will be throw when this scenario is detected
					// 
					// first find all the records with the start date between the start date and the end date of the new record (inclusive)
					// we should find at least 1 the series is built correctly.
					var after = collection
						.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate && args.StartDate <= src.EndDate)
						.ToArray();
					if (after.Length == 0) {
						throw new InvalidOperationException($"Cannot insert date level item at the end of time series or within the start date and the end date of an existing date level entry");
					} else {
						foreach (var item in after) {
							// if the end date of the found record is before the end date of the new record, simply remove the found record
							if (item.EndDate <= src.EndDate) {
								collection.Remove(item);
							} else {
								// here a record has been found to overlap the end date of the new record
								// only 1 of this kind of record should be found
								var changed = !item.HasSameValue(src);
								if (changed) {
									// if the value is different, set the start date of the current record to be the end date + 1 of the new record
									item.StartDate = src.EndDate.AddDays(1);
									collection.Add(src);
								} else {
									// if the value is the same as the new record, simply change the start date of the current record
									// remember to set it as the new record
									item.StartDate = src.StartDate;
									src = item;
								}
							}
						}
					}
				}
			} else {
				// this code path here will always have the end date of the new record to MaxEndDate
				var items = collection.Where(args => args.Key.Equals(src.Key) && args.StartDate >= src.StartDate).ToArray();
				bool hasExisting = false;
				foreach (var item in items) {
					if (!hasExisting && item.HasSameValue(src)) {
						hasExisting = true;
						item.EndDate = DateLevelEntity.MaxEndDate;
						item.StartDate = src.StartDate;
						src = item;
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

		public static void UpdateDateLevel<T, K>(this ICollection<T> collection, Func<T, T> clone, Action<T> modify, DateOnly start, DateOnly end)
			where K : IEquatable<K>
			where T : DateLevelEntity<K> {
			if (start > end) {
				throw new ArgumentException("Start date cannot be greater than end date");
			}
			foreach (var item in collection.ToArray()) {
				if (item.EndDate < start || item.StartDate > end) {
					continue;
				} else if (start <= item.StartDate && item.EndDate <= end) {
					modify(item);
				} else if (item.StartDate < start && end < item.EndDate) {
					var after = clone(item);
					after.StartDate = end.AddDays(1);
					after.EndDate = item.EndDate;
					collection.Add(after);

					var newItem = clone(item);
					modify(newItem);
					newItem.StartDate = start;
					newItem.EndDate = end;
					collection.Add(newItem);

					item.EndDate = start.AddDays(-1);
				} else if (item.StartDate < start && start <= item.EndDate) {
					item.EndDate = start.AddDays(-1);
					var newItem = clone(item);
					modify(newItem);
					newItem.StartDate = start;
					newItem.EndDate = item.EndDate;
					collection.Add(newItem);
				} else if (item.StartDate <= end && end < item.EndDate) {
					var newItem = clone(item);
					modify(newItem);
					newItem.StartDate = item.StartDate;
					newItem.EndDate = end;
					collection.Add(newItem);
					item.StartDate = end.AddDays(1);
				}
			}
			RebuildDateLevelSeries(collection, args => collection.Remove(args));
		}
		//public void SetClientFactor(DateOnly fromDate, DateOnly toDate, decimal currAum, decimal newAum, string modifiedBy) {
		//	// retrieve the affected trade sizes
		//	var items = GetTradeSizesWithDateRange(fromDate, toDate);
		//	foreach (var item in items.ToList()) {
		//		// insert new client factor for the first time serie
		//		if (fromDate >= item.StartDate && fromDate <= item.EndDate) {
		//			SetTradeSize(new TradeSize(Id, currAum, newAum, item.StrategistFactor, fromDate, DateTime.UtcNow, modifiedBy));
		//		} else if (toDate >= item.StartDate && toDate <= item.EndDate) { 
		//			// insert new client factor for the last date serie
		//			var newItem = new TradeSize(Id, currAum, newAum, item.StrategistFactor, item.StartDate, DateTime.UtcNow, modifiedBy) {
		//				EndDate = toDate.AddDays(-1)
		//			};
		//			SetTradeSize(newItem);
		//		} else { // replace factor for all other date series that fall between
		//			item.ClientFactor = Math.Round(newAum/currAum, 12);
		//			item.ModifiedBy = modifiedBy;
		//		}
		//	}
		//	TradeSizes.RebuildDateLevelSeries(args => TradeSizes.Remove(args));
		//}

		/// <summary>
		/// Provided a date level series data for a single entity, the method will remove the datelevel item with the specified startDate.
		/// The method will always extend the end date of the previous record if it exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="set"></param>
		/// <param name="startDate"></param>
		/// <param name="remove"></param>
		public static void DeleteDateLevel<T>(this IEnumerable<T> set, DateOnly startDate, Action<T> remove)
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
			T? current = null;
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
		public static IEnumerable<T> GetDateLevelEntityByDate<T>(this IEnumerable<T> source, DateOnly effectiveDate)
			where T : DateLevelEntity {
			var items = source.Where(args => args.StartDate <= effectiveDate && args.EndDate >= effectiveDate).ToArray();
			return items;
		}

		public static T? GetDateLevelItemByDate<T, K>(this IEnumerable<T> source, K key, DateOnly effectiveDate)
			where T : DateLevelEntity<K> where K : IEquatable<K> {
			var item = source.Where(args => args.Key.Equals(key)
				&& args.StartDate <= effectiveDate
				&& args.EndDate >= effectiveDate).FirstOrDefault();
			return item;
		}

		/// <summary>
		/// Provided a date level series and a date range, this method will search and return items
		/// where the date range falls between the start and end dates.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <returns></returns>
		public static IEnumerable<T> GetDateLevelEntityByDateRange<T>(this IEnumerable<T> source, DateOnly fromDate, DateOnly toDate)
			where T : DateLevelEntity {
			return source.Where(args => !(fromDate > args.EndDate || toDate < args.StartDate));
		}
	}
}
#endif