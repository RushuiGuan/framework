using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Albatross.Repository.TimeSeries {
	public class TimeSeriesDto {
		public int Id { get; set; }
		public DateTime? Start { get; set; }
		public DateTime? End { get; set; }
	}

	public abstract class TimeSeriesEntity<T, Dto> : ImmutableEntity where T : TimeSeriesEntity<T, Dto>, new() where Dto: TimeSeriesDto {

		public readonly static DateTime MinimumDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
		public readonly static DateTime MaximumDate = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;

		public int Id { get; private set; }
		public DateTime Start { get; private set; }
		public DateTime End { get; private set; }
		public abstract Expression<Func<T, bool>> SeriesExpression { get; }
		public abstract bool CanMerge(T item);

		public virtual T Clone(string user) {
			return new T {
				Start = Start,
				End = End,
				CreatedBy = user,
				CreatedUTC = DateTime.UtcNow,
			};
		}


		protected virtual void Create(Dto src, string user, IDbSession session) {
			Start = src.Start ?? MinimumDate;
			End = src.End ?? MaximumDate;
			var set = session.DbContext.Set<T>();
			var items = set.Where(SeriesExpression).ToArray();

			var replacing = items.Where(args => Start <= args.Start && args.End <= End);
			set.RemoveRange(replacing);

			var front = items.Where(args => args.Start < Start && args.End >= Start && args.End <= End);
			foreach (var item in front) {
				if (CanMerge(item)) {
					set.Remove(item);
					if (item.Start < Start) {
						Start = item.Start;
					}
				} else {
					item.End = Start.AddDays(-1);
				
				}
			}

			var back = items.Where(args => Start  <= args.Start  && args.Start <= End && End < args.End);
			foreach (var item in back) {
				if (CanMerge(item)) {
					set.Remove(item);
					if (item.End > End) {
						End = item.End;
					}
				} else {
					item.Start = End.AddDays(1);
				}
			}

			var overlap = items.Where(args => args.Start < Start && args.End > End);
			foreach (var item in overlap) {
				if (CanMerge(item)) {
					set.Remove(item);
					if (item.Start < Start) { Start = item.Start; }
					if (item.End > End) { End = item.End; }
				} else {
					var clone = item.Clone(user);
					item.End = Start.AddDays(-1);
					clone.Start = End.AddDays(1);
					set.Add(clone);
				}
			}
			base.Create(user);
		}

		public override void Validate() {
			base.Validate();
			if (Start > End) {
				throw new ValidationException("Start time has to be before end time");
			}
		}
	}
}
