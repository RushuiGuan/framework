using System;

namespace Albatross.EFCore {
	public interface IJsonProperty<T> : IEquatable<T> where T:IJsonProperty<T>{
		T Snapshot();
	}
}
