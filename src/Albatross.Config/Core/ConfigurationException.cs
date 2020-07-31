using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.Core {
	public class ConfigurationException : Exception{
		public ConfigurationException(Type type, string property, string msg) : base($"Invalid Config Value: {type.FullName}-{property}; {msg}") { }
		public ConfigurationException(Type type, string property): base($"Missing Config Value: {type.FullName}-{property}") { }
		public ConfigurationException(string key):base($"Missing Config Data with Key: {key}") { }
	}
}
