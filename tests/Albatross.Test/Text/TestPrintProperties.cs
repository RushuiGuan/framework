﻿using Albatross.Text;
using System;
using System.IO;
using Xunit;

namespace Albatross.Test.Text {
	public class TestPrintProperties {
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
		const string expectedDefaultOptions =
@"           Name apple                  orange                 desk                  
       Cateogry fruits                                        furniture             
         Weight 1000                   800                    200                   
CreatedDateTime 2000-01-01 23:01:50:-5 2000-07-01 02:03:03:-4 2000-02-01 05:05:04:-5
        Expired 2000-07-02             2000-07-03             2000-07-04            
";
		[Fact]
		public void TestWithDefaultOptions() {
			StringWriter writer = new StringWriter();
			var products = GetProducts();
			writer.PrintProperties<Product>(products, new PrintPropertiesOption(), nameof(Product.Name), nameof(Product.Cateogry), nameof(Product.Weight), nameof(Product.CreatedDateTime), nameof(Product.Expired));
			Assert.Equal(expectedDefaultOptions, writer.ToString());
		}
		
		const string expectedWithHeaders =
@"                My Product             His Product            Her Product           
------------------------------------------------------------------------------------
           Name apple                  orange                 desk                  
       Cateogry fruits                                        furniture             
         Weight 1000                   800                    200                   
CreatedDateTime 2000-01-01 23:01:50:-5 2000-07-01 02:03:03:-4 2000-02-01 05:05:04:-5
        Expired 2000-07-02             2000-07-03             2000-07-04            
";
		[Fact]
		public void TestWithHeader() {
			StringWriter writer = new StringWriter();
			var products = GetProducts();
			writer.PrintProperties<Product>(products, new PrintPropertiesOption {
				GetHeader = args => {
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
				}
			}, nameof(Product.Name), nameof(Product.Cateogry), nameof(Product.Weight), nameof(Product.CreatedDateTime), nameof(Product.Expired));
			Assert.Equal(expectedWithHeaders, writer.ToString());
		}

	}
}
