using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	//public abstract record class DateLevelEntity<EntityType, DateLevelDataType> where EntityType: DateLevelEntity<EntityType, DateLevelDataType> where DateLevelDataType: DateLevelData<EntityType> {
	//	public int Id { get; init; }
	//	public DateTime CreatedUtc { get; init; }
	//	[MaxLength(Constant.UserNameLength)]
	//	public string CreatedBy { get; init; }

	//	public List<DateLevelDataType> DataLevel { get; init; } = new List<DateLevelDataType>();

	//	public DateLevelEntity(string createdBy) {
	//		this.CreatedUtc = DateTime.UtcNow;
	//		this.CreatedBy = createdBy;
	//	}
	//	public DateLevelEntity(int id, DateTime createdUtc, string createdBy) {
	//		Id = id;
	//		CreatedUtc = createdUtc;
	//		CreatedBy = createdBy;
	//	}


	//	public async Task Set(DateLevelDataType data, DbSet<DateLevelDataType> set) {
	//		var items = await set.Where(args => args.Id == Id 
	//			&& (args.StartDate >= data.StartDate && args.StartDate <= data.EndDate 
	//				|| args.EndDate >= data.StartDate && args.EndDate <= data.EndDate
	//				|| args.StartDate < data.StartDate && args.EndDate > data.EndDate
	//			)).ToArrayAsync();
	//		foreach(var item in items) {
	//			if(item.StartDate < data.StartDate && item.EndDate > data.EndDate) {
	//				var copy = item with {
	//					StartDate = data.EndDate.AddDays(1),
	//				};
	//				item.EndDate = data.StartDate.AddDays(-1);
	//				set.Add(copy);
	//			}
	//		}
	//	}
	//}

	public abstract record class DateLevelEntity {
		public int Id { get; init; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public readonly static DateTime MaxEndDate = new DateTime(9999, 12, 31);

		public DateTime CreatedUtc { get; init; }
		[MaxLength(Constant.UserNameLength)]
		public string CreatedBy { get; init; }

		public DateTime ModifiedUtc { get; set; }
		[MaxLength(Constant.UserNameLength)]
		public string ModifiedBy { get; set; }

		public abstract void Update(DateLevelEntity src);

		// contructor used by efcore
		protected DateLevelEntity(int id, DateTime startDate, DateTime endDate, DateTime createdUtc, string createdBy, DateTime modifiedUtc, string modifiedBy) {
			this.Id = id;
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.CreatedBy = createdBy;
			this.CreatedUtc = createdUtc;
			this.ModifiedUtc = modifiedUtc;
			this.ModifiedBy = modifiedBy;
		}

		public DateLevelEntity(int id, DateTime startDate, DateTime? endDate, string createdBy) {
			this.Id = id;
			this.StartDate = startDate;
			this.EndDate = endDate ?? MaxEndDate;
			this.CreatedBy = createdBy;
			this.CreatedUtc = DateTime.UtcNow;
			this.ModifiedBy = createdBy;
			this.ModifiedUtc = DateTime.UtcNow;
		}
	}
}