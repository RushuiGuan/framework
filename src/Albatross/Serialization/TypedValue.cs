﻿using Albatross.Reflection;
using System.Text.Json.Serialization;

namespace Albatross.Serialization {
	[JsonConverter(typeof(TypedValueJsonConverter<TypedValue>))]
	public class TypedValue {
		public string ClassName { get; set; }
		public object Value { get; set; }
	}
}
