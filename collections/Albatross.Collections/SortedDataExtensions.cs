using System;
using System.Threading.Tasks;

namespace Albatross.Collections {
	public static class SortedDataExtensions {
		/// <summary>
		/// Return true if changed data are to the left of the current data, no overlaps
		/// </summary>
		public static bool Leftie<T>(this ISortedData<T> current, ISortedData<T> changes) where T : IComparable<T>
			=> changes.LastKey.CompareTo(current.FirstKey) < 0;


		/// <summary>
		/// Return true if changed data are to the right of the current data, no overlaps
		/// </summary>
		public static bool Rightie<T>(this ISortedData<T> current, ISortedData<T> changes) where T : IComparable<T>
			=> current.LastKey.CompareTo(changes.FirstKey) < 0;

		/// <summary>
		/// Return true if changed data overlaps the current data.  The first key of the changed data is less than or equal to 
		/// the first key of the current data, and the last key of the changed data is greater than or equal to the last key of 
		/// the current data.
		/// </summary>
		public static bool WrappBy<T>(this ISortedData<T> current, ISortedData<T> changes) where T : IComparable<T>
			=> changes.FirstKey.CompareTo(current.FirstKey) <= 0 && current.LastKey.CompareTo(changes.LastKey) <= 0;

		/// <summary>
		/// Return true if the current data contains the changed data.  THe first key of the changed data is less than the first 
		/// key of the current data, and the last key of the changed data is less than the last key of the current data.
		/// </summary>
		public static bool Contains<T>(this ISortedData<T> current, ISortedData<T> changes) where T : IComparable<T>
			=> current.FirstKey.CompareTo(changes.FirstKey) < 0 && changes.LastKey.CompareTo(current.LastKey) < 0;

		/// <summary>
		/// Return true if the changed data are to the left of the current data, with overlaps
		/// </summary>
		public static bool LeftieWithOverlap<T>(this ISortedData<T> current, ISortedData<T> changes) where T : IComparable<T> =>
			changes.FirstKey.CompareTo(current.FirstKey) <= 0
			&& changes.LastKey.CompareTo(current.FirstKey) >= 0
			&& changes.LastKey.CompareTo(current.LastKey) < 0;

		/// <summary>
		/// Return true if the changed data are to the right of the current data, with overlaps
		/// </summary>
		public static bool RightieWithOverlap<T>(this ISortedData<T> current, ISortedData<T> changes) where T : IComparable<T> =>
			current.FirstKey.CompareTo(changes.FirstKey) < 0
			&& changes.FirstKey.CompareTo(current.LastKey) <= 0
			&& current.LastKey.CompareTo(changes.LastKey) <= 0;

		public static async Task Stitch<T>(this ISortedData<T> current, ISortedData<T> changes) where T : IComparable<T> {
			if (!changes.Any()) {
				return;
			} else if (!current.Any()) {
				current.ResetPosition();
				changes.ResetPosition();
				await current.ReplacedBy(changes);
			} else {
				current.ResetPosition();
				changes.ResetPosition();
				if (current.Rightie(changes)) {
					current.Seek(new EndPosition());
					await current.Append(changes);
				} else if (current.Leftie(changes)) {
					await current.Prepend(changes);
				} else if (current.WrappBy(changes)) {
					await current.ReplacedBy(changes);
				} else if (current.Contains(changes)) {
					// append second gap to the changes
					IPosition? index = null;
					while (current.TryReadNexKey(out var key, out var priorPosition)) {
						if (key.CompareTo(changes.FirstKey) >= 0) {
							if (index == null) {
								index = priorPosition;
							}
						}
						if (key.CompareTo(changes.LastKey) > 0) {
							current.Seek(priorPosition);
							break;
						}
					}
					changes.Seek(new EndPosition());
					await changes.Append(current);
					current.Seek(index!);
					changes.ResetPosition();
					await current.Append(changes);
				} else if (current.LeftieWithOverlap(changes)) {
					while (current.TryReadNexKey(out var key, out var priorPosition)) {
						if (key.CompareTo(changes.LastKey) > 0) {
							current.Seek(priorPosition);
							break;
						}
					}
					// reset the change current so that we prepend the whole file to the current file
					await current.Prepend(changes);
				} else if (current.RightieWithOverlap(changes)) {
					// overlapped after, exclusive of the first key, inclusive of the last key
					while (current.TryReadNexKey(out var key, out var priorPosition)) {
						if (key.CompareTo(changes.FirstKey) >= 0) {
							current.Seek(priorPosition);
							break;
						}
					}
					await current.Append(changes);
				}
			}
		}
	}
}