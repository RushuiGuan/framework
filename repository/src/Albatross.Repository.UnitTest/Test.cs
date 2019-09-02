using System.Linq;
using Albatross.Host.NUnit;
using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Albatross.Repository.UnitTest.Model;
using Albatross.Repository.UnitTest.Repository;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Albatross.Repository.UnitTest.Dto;
using Albatross.Mapping.Core;
using System.Threading.Tasks;

namespace Albatross.Repository.UnitTest {
	[TestFixture]
	public class Test : TestBase<TestUnitOfWork> {

		public override void RegisterPackages(IServiceCollection svc) {
			svc.AddTestDatabase();
		}

		private void CreateAddress(ContactDto c) {
			const int AddressCount = 10;
			c.Addresses = new AddressDto[AddressCount];
			for(int i=0; i<AddressCount; i++) {
				c.Addresses[i] = new AddressDto {
					City = $"c{i}",
					State = c.Name,
					Street = $"s{i}",
				};
			}
		}
		private Task RemoveContact(string name) {
			using (var unitOfWork = NewUnitOfWork()) {
				unitOfWork.Get<TestingDbContext>().Migrate();
				var contacts = unitOfWork.Get<ContactRepository>();
				if (contacts.TryGet(name, out Contact item)) {
					contacts.Remove(item);
				}
				return contacts.SaveChangesAsync();
			}
		}
		private ContactDto GetContact(string name, out Contact model) {
			using (var unitOfWork = NewUnitOfWork()) {
				unitOfWork.Get<TestingDbContext>().Migrate();
				var contacts = unitOfWork.Get<ContactRepository>();
				model = contacts.Get(name);
				return unitOfWork.Get<IMapperFactory>().Map<Contact, ContactDto>(model);
			}
		}
		private async Task<ContactDto> CreateContact(string name, int user) {
			Contact model;
			using (var unitOfWork = NewUnitOfWork()) {
				unitOfWork.Get<TestingDbContext>().Migrate();
				var contacts = unitOfWork.Get<ContactRepository>();
				var dto = new ContactDto {
					Name= name,
				};
				CreateAddress(dto);
				model = new Contact(dto, user);
				contacts.Add(model);
				await contacts.SaveChangesAsync();
				return unitOfWork.Get<IMapperFactory>().Map<Contact, ContactDto>(model);
			}
		}
		private async Task<ContactDto> UpdateContact(ContactDto dto, int user) {
			Contact model;
			using (var unitOfWork = NewUnitOfWork()) {
				unitOfWork.Get<TestingDbContext>().Migrate();
				var contacts = unitOfWork.Get<ContactRepository>();
				model = contacts.Get(dto.Name);
				model.Update(dto, user);
				await contacts.SaveChangesAsync();
				return unitOfWork.Get<IMapperFactory>().Map<Contact, ContactDto>(model);
			}
		}

		[Test]
		public async Task Create() {
			string name = nameof(Create);
			await RemoveContact(name);
			var dto = await CreateContact(name, 1);
			using (var unitOfWork = NewUnitOfWork()) {
				unitOfWork.Get<TestingDbContext>().Migrate();
				var contacts = unitOfWork.Get<ContactRepository>();
				var c = contacts.Get(name);
				Assert.AreEqual(1, c.CreatedBy);
				Assert.AreEqual(1, c.ModifiedBy);
				Assert.AreEqual(dto.Name, c.Name);
				Assert.AreEqual(dto.CreatedBy, c.CreatedBy);
				Assert.AreEqual(dto.ModifiedBy, c.ModifiedBy);
			}
		}

		[Test]
		public async Task Update() {
			string name = nameof(Update);
			await RemoveContact(name);
			var dto = await CreateContact(name, 1);
			for (int i = 5; i < 10; i++) {
				dto.Addresses[i].City = name;
			}
			dto = await UpdateContact(dto, 2);

			Assert.AreEqual(1, dto.CreatedBy);
			Assert.AreEqual(2, dto.ModifiedBy);
		}
	}
}
