using System;
using System.Linq.Expressions;

namespace Albatross.Repository.Core {
	public class EntityNotFoundException : Exception {
		public EntityNotFoundException(string msg) : base(msg) { }
	}

	public class EntityNotFoundException<TSource> : EntityNotFoundException {
		public EntityNotFoundException(string property, object? value) : base(GetMsg(property, value)) { }

		static string GetMsg(string property, object? value) {
			if (value == null) {
				return $"Cannot find {typeof(TSource).Name} with a null value for {property}";
			} else {
				return $"Cannot find {typeof(TSource).Name} with the {property} of {value}";
			}
		}
	}
}