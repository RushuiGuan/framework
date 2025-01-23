using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace Albatross.Serialization.Yaml {
	public static class Extensions {
		public static ISerializer DefaultYamlSerializer() {
			var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance)
				.ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
#if NET6_0_OR_GREATER
				.WithTypeConverter(new DateOnlyTypeConverter())
#endif
				.WithTypeConverter(new DateTimeTypeConverter())
				.WithTypeConverter(new JsonElementTypeConverter())
				.Build();
			return serializer;
		}
	}
}
