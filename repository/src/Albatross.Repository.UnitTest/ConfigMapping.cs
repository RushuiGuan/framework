using Albatross.Repository.UnitTest.Dto;
using Albatross.Repository.UnitTest.Model;
using AutoMapper;

namespace Albatross.Repository.UnitTest {
	public class ConfigMapping : Albatross.Mapping.Core.IConfigMapping {
		public void Configure(IMapperConfigurationExpression expression) {
			expression.CreateMap<Contact, ContactDto>();
			expression.CreateMap<Address, AddressDto>();
		}
	}
}
