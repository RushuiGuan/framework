using Albatross.Repository.Core;
using Castle.DynamicProxy.Contributors;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Albatross.Repository.TimeSeries.Test
{
	public class TestTimeSeriesModel
	{
		IDbSession SetupFundEquity(DateTime effectiveDate, int fundId) {
			var options = new DbContextOptionsBuilder<FundEquityDbSession>().UseInMemoryDatabase("test").Options;
			var session = new FundEquityDbSession(options);
			for (int i = 1; i <= 12; i++) {
				session.DbContext.Set<FundEquity>().Add(
					new FundEquity(
						new FundEquityDto {
							EffectiveDate = effectiveDate,
							FundId = fundId,
							Nav = i * 1000,
							Start = new DateTime(2020, i, 1),
							End = new DateTime(2020, i, 15),
						}, "test", session));
			}
			session.SaveChanges();
			return session;
		}

		[Fact]
		public async Task TestNoOverLapAtTheEnd() {
			DateTime effectiveDate = DateTime.Today;
			int fundId = 100;
			using var session = SetupFundEquity(DateTime.Today, fundId);
			var src = new FundEquityDto {
				EffectiveDate = effectiveDate,
				FundId = fundId,
				Start = new DateTime(2021, 1, 1),
			};
			var entity = new FundEquity(src, "test", session);
			session.DbContext.Add(entity);
			await session.SaveChangesAsync();
			var list = session.DbContext.Set<FundEquity>().ToList();
			Assert.Equal(13, list.Count);
			Assert.Same(entity, list.Last());
		}

		[Fact]
		public async Task TestNoOverLapAtTheBeginning() {
			DateTime effectiveDate = DateTime.Today;
			int fundId = 100;
			using var session = SetupFundEquity(DateTime.Today, fundId);
			var src = new FundEquityDto {
				EffectiveDate = effectiveDate,
				FundId = fundId,
				End = new DateTime(1999, 1, 1),
			};
			var entity = new FundEquity(src, "test", session);
			session.DbContext.Add(entity);
			await session.SaveChangesAsync();
			var list = session.DbContext.Set<FundEquity>().ToList();
			Assert.Equal(13, list.Count);
			Assert.Same(entity, list.Last());
		}

		[Fact]
		public async Task TestNoOverLapInTheMiddle() {
			DateTime effectiveDate = DateTime.Today;
			int fundId = 100;
			using var session = SetupFundEquity(DateTime.Today, fundId);
			var src = new FundEquityDto {
				EffectiveDate = effectiveDate,
				FundId = fundId,
				Start = new DateTime(2000, 6, 16),
				End = new DateTime(2000, 6, 20),
			};
			var entity = new FundEquity(src, "test", session);
			session.DbContext.Add(entity);
			await session.SaveChangesAsync();
			var list = session.DbContext.Set<FundEquity>().ToList();
			Assert.Equal(13, list.Count);
			Assert.Same(entity, list.Last());
		}

		[Fact]
		public async Task TestOverLapAtTheEnd() {
			DateTime effectiveDate = DateTime.Today;
			int fundId = 100;
			using var session = SetupFundEquity(DateTime.Today, fundId);
			var src = new FundEquityDto {
				EffectiveDate = effectiveDate,
				FundId = fundId,
				Start = new DateTime(2020, 12, 10),
			};
			var entity = new FundEquity(src, "test", session);
			session.DbContext.Add(entity);
			await session.SaveChangesAsync();
			var list = session.DbContext.Set<FundEquity>().ToList();
			Assert.Equal(13, list.Count);
			Assert.Same(entity, list.Last());
			var item = list[11];
			Assert.Equal(new DateTime(2020, 12, 9), item.End);
		}

		[Fact]
		public async Task TestOverLapAtTheBeginning() {
			DateTime effectiveDate = DateTime.Today;
			int fundId = 100;
			using var session = SetupFundEquity(DateTime.Today, fundId);
			var src = new FundEquityDto {
				EffectiveDate = effectiveDate,
				FundId = fundId,
				End = new DateTime(2020, 1, 5),
			};
			var entity = new FundEquity(src, "test", session);
			session.DbContext.Add(entity);
			await session.SaveChangesAsync();
			var list = session.DbContext.Set<FundEquity>().ToList();
			Assert.Equal(13, list.Count);
			Assert.Same(entity, list.Last());
			var item = list[0];
			Assert.Equal(new DateTime(2020, 1, 6), item.Start);
		}

		[Fact]
		public async Task TestOverLapInTheMiddle() {
			DateTime effectiveDate = DateTime.Today;
			int fundId = 100;
			using var session = SetupFundEquity(DateTime.Today, fundId);
			var src = new FundEquityDto {
				EffectiveDate = effectiveDate,
				FundId = fundId,
				Start = new DateTime(2020, 1, 5),
				End = new DateTime(2020, 1, 5),
			};
			var entity = new FundEquity(src, "test", session);
			session.DbContext.Add(entity);
			await session.SaveChangesAsync();
			var list = session.DbContext.Set<FundEquity>().ToList();
			Assert.Equal(14, list.Count);
			var item = list[0];
			Assert.Equal(new DateTime(2020, 1, 1), item.Start);
			Assert.Equal(new DateTime(2020, 1, 4), item.End);
			item = list[12];
			Assert.Equal(new DateTime(2020, 1, 6), item.Start);
			Assert.Equal(new DateTime(2020, 1, 15), item.End);
			Assert.Same(entity, item);
			item = list[13];
			Assert.Equal(new DateTime(2020, 1, 5), item.Start);
			Assert.Equal(new DateTime(2020, 1, 5), item.End);
		}
	}
}
