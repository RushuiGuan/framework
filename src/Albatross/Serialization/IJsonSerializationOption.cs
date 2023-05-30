using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Albatross.Serialization {
	public interface IJsonSerializationOption {
		JsonSerializerOptions Default { get; }
		JsonSerializerOptions Alternate { get; }
	}
}
