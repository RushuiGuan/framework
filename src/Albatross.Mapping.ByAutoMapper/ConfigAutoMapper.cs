using Albatross.Mapping.Core;
using System.Collections.Generic;

namespace Albatross.Mapping.ByAutoMapper {
	public class ConfigAutoMapper {
		IEnumerable<IConfigMapping> configurations;
        IEnumerable<AutoMapper.Profile> profiles;


        public ConfigAutoMapper(IEnumerable<IConfigMapping> configurations, IEnumerable<AutoMapper.Profile> profiles) {
			this.configurations = configurations;
            this.profiles = profiles;
		}


		public AutoMapper.IMapper Create() {
			var cfg = new AutoMapper.MapperConfiguration(Configure);
			return cfg.CreateMapper();
		}

		void Configure(AutoMapper.IMapperConfigurationExpression expression) {
            expression.AddProfiles(this.profiles);
			foreach (var item in this.configurations) {
				item.Configure(expression);
			}
		}
	}
}
