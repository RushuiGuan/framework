using msg = Albatross.CRM.Messages;
using Albatross.CRM.Model;
using AutoMapper;

namespace Albatross.Repository.Dto{
	public class ConfigMapping : Albatross.Mapping.ByAutoMapper.IConfigMapping{
		public void Configure(IMapperConfigurationExpression expression) {
			expression.CreateMap<Contact, msg.Contact>();
			expression.CreateMap<Address, msg.Address>();
			expression.CreateMap<Customer, msg.Customer>();
		}
	}
}
