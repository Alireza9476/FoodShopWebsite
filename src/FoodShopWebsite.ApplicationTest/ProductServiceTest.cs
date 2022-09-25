using FoodShopWebsite.Application.ProductApp;
using FoodShopWebsite.Dtos;
using FoodShopWebsite.Infrastructure;
using FoodShopWebsite.Domain.Exceptions;

using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace FoodShopWebsite.ApplicationTest
{
    public class ProductServiceTest
    {
        private TestFoodShopWebsiteDbContext GenerateDb()
        {
            var options = new DbContextOptionsBuilder()
               .UseSqlite($"Data Source=TestAdministrator_Test.db")
               .Options;

            var db = new TestFoodShopWebsiteDbContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.SeedDatabase();
            return db;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////

        [Fact]
        public void Create_NameToShort_ProductServiceCreateException_Test()
        {
            TestFoodShopWebsiteDbContext db = GenerateDb();

            ProductDto newProductDto = new ProductDto()
            {
                CategorieGuid = new Guid("64A862F7-CEFE-4DEF-A315-11F5061943AA"),
                ProductDetailsGuid = new Guid("C10C3DCC-41F9-43B4-A0FB-EA42E8435E93"),
                DiscountProductGuid = new Guid("A1E2230D-9036-4B4F-9D09-27329A74ACD6"),
                Price = 20,
                Name = "Ap"
            };

            Exception ex = Assert.Throws<ServiceValidationException>(
                () => new ProductService(
                    null, null, null, null
                    //new IRepositoryBase<Product>(db),
                    //new IRepositoryBase<DiscountProduct>(db),
                    //new IRepositoryBase<Categorie>(db),
                    //new IRepositoryBase<ProductDetails>(db)
                    )
                .Create(newProductDto, newProductDto.DiscountProductGuid, newProductDto.CategorieGuid)
            );

            Assert.Equal("Name muss mindestens 3 Zeichen lang sein", ex.Message);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////

        [Fact]
        public void Create_NameToLong_ProductServiceCreateException_Test()
        {
            TestFoodShopWebsiteDbContext db = GenerateDb();

            ProductDto newProductDto = new ProductDto()
            {
                CategorieGuid = new Guid("64A862F7-CEFE-4DEF-A315-11F5061943AA"),
                ProductDetailsGuid = new Guid("C10C3DCC-41F9-43B4-A0FB-EA42E8435E93"),
                DiscountProductGuid = new Guid("A1E2230D-9036-4B4F-9D09-27329A74ACD6"),
                Price = 20,
                Name = "ApplesAreTasty"
            };

            Exception ex = Assert.Throws<ServiceValidationException>(
                () => new ProductService(
                        //new IRepositoryBase<Product>(db),
                        //new IRepositoryBase<DiscountProduct>(db),
                        //new IRepositoryBase<Categorie>(db),
                        //new IRepositoryBase<ProductDetails>(db)
                        null, null, null, null
                    )
                .Create(newProductDto, newProductDto.DiscountProductGuid, newProductDto.CategorieGuid)
            );

            Assert.Equal("Name muss maximal 10 Zeichen lang sein", ex.Message);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////

        [Fact]
        public void Create_PriceToHeigh_ProductServiceCreateException_Test()
        {
            TestFoodShopWebsiteDbContext db = GenerateDb();

            ProductDto newProductDto = new ProductDto()
            {
                CategorieGuid = new Guid("AB854EA6-2F30-42DB-B8CF-5A1BB80F10E6"),
                ProductDetailsGuid = new Guid("653BAE4D-14F1-459B-B829-4C71C09526E3"),
                DiscountProductGuid = new Guid("8006441A-96B3-4297-853A-0D2D65054B0C"),
                Price = 100000,
                Name = "Birne"
            };

            Exception ex = Assert.Throws<ServiceValidationException>(
                () => new ProductService(
                        //new IRepositoryBase<Product>(db),
                        //new IRepositoryBase<DiscountProduct>(db),
                        //new IRepositoryBase<Categorie>(db),
                        //new IRepositoryBase<ProductDetails>(db)
                        null, null, null, null
                    )
                .Create(newProductDto, newProductDto.DiscountProductGuid, newProductDto.CategorieGuid)
            );

            Assert.Equal("Der Preis darf 10000 nicht überschreiten", ex.Message);
        }
    }
}