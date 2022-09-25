using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FoodShopWebsite.Infrastructure.Test
{
    public class UnitTest1
    {
        [Fact]
        public void GenerateDb()
        {
            var options = new DbContextOptionsBuilder().UseSqlite($"Data Source=FoodShopWebsite.db").Options;

            var db = new TestFoodShopWebsiteDbContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.SeedDatabase();

            Assert.True(true);
        }
    }
}