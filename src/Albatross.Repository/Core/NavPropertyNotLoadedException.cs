using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.Core {
	public class NavPropertyNotLoadedException : Exception{
	public NavPropertyNotLoadedException(Type type, string property):base($"Nav property {property} of type {type.FullName} is not loaded"){ }
	}
}
