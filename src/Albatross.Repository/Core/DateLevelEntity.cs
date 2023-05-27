﻿using System;

namespace Albatross.Repository.Core {
	public abstract class DateLevelEntity : ICloneable {
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; } = MaxEndDate;
		public readonly static DateTime MaxEndDate = new DateTime(9999, 12, 31);

		public abstract bool HasSameValue(DateLevelEntity src);

		public virtual object Clone() {
			return this.MemberwiseClone();
		}

		public DateLevelEntity(DateTime startDate) {
			this.StartDate = startDate;
		}
	}

	public abstract class DateLevelEntity<K> : DateLevelEntity where K : IEquatable<K> {
		public abstract K Key { get; }
		public DateLevelEntity(DateTime startDate) : base(startDate) { }
	}
}