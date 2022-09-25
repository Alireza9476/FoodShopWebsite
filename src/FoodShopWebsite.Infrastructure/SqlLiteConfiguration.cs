using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodShopWebsite.Infrastructure
{
    public static class SqlLiteConfiguration        
    {
        //static, weil extension Methods logischerweise statisch zugegriffen werden, zb UseSqlite ist auch extention Methode.
        //Wir erweitern IServiceColletion mit ConfigureSqlLite() Methode -> extention Method, this als Parameter soll um diesen TYPE erweitert werden.
        //Infrastrcuture darf Presentation (MVC) nicht direkt zugreifen -> DbContext (ebenso müsste ich die NuGet Pakete installieren)
        //static und this beachten
        public static void ConfigureSqlLite(this IServiceCollection services){
            services.AddDbContext<TestFoodShopWebsiteDbContext>(options => options.UseSqlite($"Data Source=FoodShopWebsite.db"));
        }
    }
}
