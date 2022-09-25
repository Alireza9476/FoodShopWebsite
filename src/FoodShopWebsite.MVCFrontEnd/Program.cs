using FoodShopWebsite.Application.CategorieApp;
using FoodShopWebsite.Application.DiscountProductApp;
using FoodShopWebsite.Application.DiscountProducts;
using FoodShopWebsite.Application.ProductApp;
using FoodShopWebsite.Application.UserApp;
using FoodShopWebsite.Domain.Model;
using FoodShopWebsite.Infrastructure;
using FoodShopWebsite.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Service -> Alle Instanzen sind in der Kübel drin, muss sie nur noch verwenden
builder.Services.ConfigureSqlLite();

//User
builder.Services.AddTransient<IUserService, UserService>();   //pro Request wird eine neue Instanz erstellt
builder.Services.AddTransient<IRepositoryBase<User>, RepositoryBase<User>>();

//Product
builder.Services.AddTransient <IProductService, ProductService>();
builder.Services.AddTransient<IRepositoryBase<Product>, RepositoryBase<Product>>();

//DiscountProductService
builder.Services.AddTransient<IDiscountProductService, DiscountProductService>();
builder.Services.AddTransient<IRepositoryBase<DiscountProduct>, RepositoryBase<DiscountProduct>>();

//CategorieService
builder.Services.AddTransient<ICategorieService, CategorieService>();
builder.Services.AddTransient<IRepositoryBase<Categorie>, RepositoryBase<Categorie>>();

//Categorie
builder.Services.AddTransient<IRepositoryBase<ProductDetails>, RepositoryBase<ProductDetails>>();

var app = builder.Build();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); //Dadurch muss ich /Users/Index nicht machen, sondern nur in /Users, defaultwert ist Index

app.Run();
