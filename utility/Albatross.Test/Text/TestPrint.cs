using Albatross.Text;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Test.Text {
	public class TestPrint {
		public class Product {
			public string Name { get; set; }
			public string? Cateogry { get; set; }
			public int Weight { get; set; }
			public DateTime CreatedDateTime { get; set; }
			public DateTime Expired { get; set; }
			public Product(string name) {
				this.Name = name;
			}
		}

		public static Product[] GetProducts() {
			return new Product[] {
				new Product("apple") {
					Weight = 1000,
					Cateogry = "fruits",
					CreatedDateTime = new DateTime(2000, 1,1, 23,1,50),
					Expired = new DateTime(2000, 7,1).AddDays(1),
				},
				new Product("orange") {
					Weight = 800,
					CreatedDateTime = new DateTime(2000, 7,1, 2,3,3),
					Expired = new DateTime(2000, 7,1).AddDays(2),
				},
				new Product("desk") {
					Weight = 200,
					Cateogry = "furniture",
					CreatedDateTime = new DateTime(2000, 2,1, 5,5,4),
					Expired = new DateTime(2000, 7,1).AddDays(3),
				}
			};
		}
		const string expectedPrintPropertiesDefaultOptions =
@"           Name apple                 orange                desk                 
       Cateogry fruits                                      furniture            
         Weight 1000                  800                   200                  
CreatedDateTime 2000-01-01 23:01:50-5 2000-07-01 02:03:03-4 2000-02-01 05:05:04-5
        Expired 2000-07-02            2000-07-03            2000-07-04           
";
		[Fact]
		public async Task PrintPropertiesWithDefaultOptions() {
			StringWriter writer = new StringWriter();
			var products = GetProducts();
			await writer.PrintProperties<Product>(products, new PrintOptionBuilder<PrintPropertiesOption>()
				.Property(nameof(Product.Name), nameof(Product.Cateogry), nameof(Product.Weight), nameof(Product.CreatedDateTime), nameof(Product.Expired))
				.Build());
			Assert.Equal(expectedPrintPropertiesDefaultOptions, writer.ToString());
		}

		const string expectedPrintPropertiesWithHeaders =
@"                My Product            His Product           Her Product          
---------------------------------------------------------------------------------
           Name apple                 orange                desk                 
       Cateogry fruits                                      furniture            
         Weight 1000                  800                   200                  
CreatedDateTime 2000-01-01 23:01:50-5 2000-07-01 02:03:03-4 2000-02-01 05:05:04-5
        Expired 2000-07-02            2000-07-03            2000-07-04           
";
		[Fact]
		public async Task PrintPropertiesWithHeader() {
			StringWriter writer = new StringWriter();
			var products = GetProducts();
			await writer.PrintProperties<Product>(products, new PrintOptionBuilder<PrintPropertiesOption>()
				.Property(nameof(Product.Name), nameof(Product.Cateogry), nameof(Product.Weight), nameof(Product.CreatedDateTime), nameof(Product.Expired))
				.ColumnHeaderLineCharacter('-')
				.ColumnHeader(args => {
					switch (args) {
						case 0:
							return "My Product";
						case 1:
							return "His Product";
						case 2:
							return "Her Product";
						default:
							return null;
					}
				}).Build());
			Assert.Equal(expectedPrintPropertiesWithHeaders, writer.ToString());
		}

		const string expectedPrintTableWithDefault =
@"Name   Cateogry  Weight CreatedDateTime       Expired   
--------------------------------------------------------
apple  fruits    1000   2000-01-01 23:01:50-5 2000-07-02
orange           800    2000-07-01 02:03:03-4 2000-07-03
desk   furniture 200    2000-02-01 05:05:04-5 2000-07-04
";

		[Fact]
		public async Task PrintTableWithDefaultOptions() {
			StringWriter writer = new StringWriter();
			var products = GetProducts();
			await writer.PrintTable<Product>(products, new PrintOptionBuilder<PrintTableOption>()
				.Property(nameof(Product.Name), nameof(Product.Cateogry), nameof(Product.Weight), nameof(Product.CreatedDateTime), nameof(Product.Expired))
				.Build());
			Assert.Equal(expectedPrintTableWithDefault, writer.ToString());
		}

		const string expectedPrintTableWithoutHeader =
@"apple  fruits    1000 2000-01-01 23:01:50-5 2000-07-02
orange           800  2000-07-01 02:03:03-4 2000-07-03
desk   furniture 200  2000-02-01 05:05:04-5 2000-07-04
";
		[Fact]
		public async Task PrintTableWithoutHeader() {
			StringWriter writer = new StringWriter();
			var products = GetProducts();
			await writer.PrintTable<Product>(products, new PrintOptionBuilder<PrintTableOption>()
				.Property(nameof(Product.Name), nameof(Product.Cateogry), nameof(Product.Weight), nameof(Product.CreatedDateTime), nameof(Product.Expired))
				.PrintHeader(false).Build());
			Assert.Equal(expectedPrintTableWithoutHeader, writer.ToString());
		}
	}
}
