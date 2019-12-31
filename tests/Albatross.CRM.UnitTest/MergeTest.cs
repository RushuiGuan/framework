using System.Collections.Generic;
using Albatross.Repository.Core;
using System.Linq;
using Xunit;

namespace Albatross.Repository.UnitTest {
	public class MergeTest {
		static IEnumerable<Model> GetModels() {
			return new Model[] {

				new Model {
					FirstName = "first.name.2",
					LastName ="last.name.2",
					Age = 2,
				},
				new Model {
					FirstName = "first.name.3",
					LastName ="last.name.3",
					Age = 3,
				},
				new Model {
					FirstName = "first.name.4",
					LastName ="last.name.4",
					Age = 4,
				},
				new Model {
					FirstName = "first.name.5",
					LastName ="last.name.5",
					Age = 5,
				},
			};
		}

		static IEnumerable<ModelDto> GetModelDtos() {
			return new ModelDto[] {
				new ModelDto {
					FirstName = "first.name.1",
					LastName ="last.name.1",
					Age = 11,
				},
				new ModelDto {
					FirstName = "first.name.2",
					LastName ="last.name.2",
					Age = 22,
				},
				new ModelDto {
					FirstName = "first.name.3",
					LastName ="last.name.3",
					Age = 33,
				},
			};
		}


		public class Model {
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public int Age { get; set; }
		}
		public class ModelDto {
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public int Age { get; set; }
		}

		[Fact]
		public void MatchedTest() {
			IEnumerable<Model> models = GetModels();
			IEnumerable<ModelDto> dtos = GetModelDtos();
			List<Model> matched = new List<Model>();
			List<ModelDto> dtoMatched = new List<ModelDto>();
			models.Merge(dtos,
				msg => new { msg.FirstName, msg.LastName },
				model => new { model.FirstName, model.LastName },
				(dto, model) => { matched.Add(model); dtoMatched.Add(dto); }, null, null);
			Assert.Equal(2, matched.Count);
			Assert.Equal(2, matched.First().Age);
			Assert.Equal(3, matched.Last().Age);

			Assert.Equal(2, dtoMatched.Count);
			Assert.Equal(22, dtoMatched.First().Age);
			Assert.Equal(33, dtoMatched.Last().Age);
		}
		[Fact]
		public void NotMatchedBySourceTest() {
			IEnumerable<Model> models = GetModels();
			IEnumerable<ModelDto> dtos = GetModelDtos();
			List<Model> notMatched = new List<Model>();

			models.Merge(dtos, 
				msg => new { msg.FirstName, msg.LastName },
				model => new { model.FirstName, model.LastName },
				null, null, src => notMatched.Add(src));

			Assert.Equal(2, notMatched.Count);
			Assert.Equal(4, notMatched.First().Age);
			Assert.Equal(5, notMatched.Last().Age);
		}
		[Fact]
		public void NotMatchedByDstTest() {
			IEnumerable<Model> models = GetModels();
			IEnumerable<ModelDto> dtos = GetModelDtos();


			List<ModelDto> notMatched = new List<ModelDto>();

			models.Merge(dtos, m => new { m.FirstName, m.LastName },
				dto => new { dto.FirstName, dto.LastName },
				null, src => notMatched.Add(src), null);

			Assert.Single(notMatched);
			Assert.Equal(11, notMatched.First().Age);
		}
	}
}
