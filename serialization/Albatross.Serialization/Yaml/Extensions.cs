using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace Albatross.Serialization.Yaml {
	public static class Extensions {
		public static SerializerBuilder DefaultBuilder(this SerializerBuilder builder) =>
			builder.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
#if NET6_0_OR_GREATER
				.WithTypeConverter(new DateOnlyTypeConverter())
#endif
				.WithTypeConverter(new DateTimeTypeConverter())
				.WithTypeConverter(new JsonElementTypeConverter());

		public static ISerializer DefaultSerializer(this SerializerBuilder builder)
			=> builder.DefaultBuilder().Build();
	}
}
