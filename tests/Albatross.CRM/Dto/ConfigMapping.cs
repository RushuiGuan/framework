using Albatross.CRM.Dto;
using Albatross.CRM.Model;
using AutoMapper;

namespace Albatross.Repository.Dto{
	public class ConfigMapping : Albatross.Mapping.ByAutoMapper.IConfigMapping{
		public void Configure(IMapperConfigurationExpression expression) {
			expression.CreateMap<Contact, ContactDto>();
			expression.CreateMap<Address, AddressDto>();
			expression.CreateMap<Customer, CustomerDto>();
		}
	}
}
