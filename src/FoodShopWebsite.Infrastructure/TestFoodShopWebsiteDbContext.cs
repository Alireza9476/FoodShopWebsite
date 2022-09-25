using System;
using Bogus;
using FoodShopWebsite.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FoodShopWebsite.Infrastructure
{
    public class TestFoodShopWebsiteDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        
        public DbSet<VAT> VATs => Set<VAT>();
        public DbSet<ProductDetails> ProductDetails => Set<ProductDetails>();
        public DbSet<DiscountProduct> DiscountProducts => Set<DiscountProduct>();
        public DbSet<DiscountCodeAll> DiscountCodeAlls => Set<DiscountCodeAll>();
        public DbSet<Categorie> Categories => Set<Categorie>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Cart> Carts => Set<Cart>();
        
        private int UserLength = 30;

        public TestFoodShopWebsiteDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=FoodShopWebsite.db");
        }

        //////////////////////////////////////////////////////////////////////////
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().OwnsOne(c => c.Adresse);
            //modelBuilder.Entity<User>().HasOne(user => user.Cart).WithOne(cart => cart.User).HasForeignKey<Cart>(f => f.UserForeignkey);
            //modelBuilder.Entity<Cart>().HasOne(cart => cart.User).WithOne(user => user.Cart).HasForeignKey<User>(f => f.CartForeignkey);
        }

        public void SeedDatabase() {

            Randomizer.Seed = new Random(3001);

            Random random = new();
            DateTime time = DateTime.UtcNow;
            DateTime currentTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);

            ////////////////////////User////////////////////////////////////////////

            List<User> users = new Faker<User>("de")                                       //Cart
                              .CustomInstantiator(f => new User(Guid.NewGuid(), f.Name.FirstName(), String.Empty, String.Empty,
                                                                f.PickRandom<Gender>(), f.Phone.PhoneNumber(),
                                                                currentTime, currentTime, new Adresse(f.Address.StreetName(), f.Address.ZipCode(), f.Address.City())
                                                 ))
                              // new Adresse(f.Address.StreetName(), f.Address.ZipCode(), f.Address.City()),
                              .Rules((f, u) =>
                              {
                                  if (u.Gender == Gender.Male)
                                      u.SecondName = f.Name.LastName(Bogus.DataSets.Name.Gender.Male);
                                  else
                                      u.SecondName = f.Name.LastName(Bogus.DataSets.Name.Gender.Female);

                                  u.Email = f.Internet.Email(u.FirstName, u.SecondName);
                              })
                              .Generate(UserLength)
                              .ToList();
            Users.AddRange(users);
            SaveChanges();

            //////////////////////////VAT//////////////////////////////////////////////

            int[] vatPerProduct = new int[] { 10, 15, 20 };

            List<VAT> vats = new Faker<VAT>("de")
                .CustomInstantiator(f => new VAT(Guid.NewGuid(), "AT", f.Random.ListItem(vatPerProduct)))
                .Generate(3)
                .ToList();
            VATs.AddRange(vats);
            SaveChanges();

            ///////////////////////////ProductDetails/////////////////////////////////////////////

            decimal[] productDetailsWeight = new decimal[] { 200, 500, 1000, 2000 };
            decimal[] productDetailsKcal = new decimal[] { 100, 200, 500 };
            decimal[] productDetailsSugarCarb = new decimal[] { 10, 20, 30, 50 };

            List<ProductDetails> productDetails = new Faker<ProductDetails>("de")
                .CustomInstantiator(f => new ProductDetails(Guid.NewGuid(), f.Random.ListItem(productDetailsWeight), f.Random.ListItem(productDetailsKcal), f.Random.ListItem(productDetailsSugarCarb), f.Random.ListItem(productDetailsSugarCarb)))
                .Generate(30)
                .ToList();
            ProductDetails.AddRange(productDetails);
            SaveChanges();

            ////////////////////////////DiscountProduct///////////////////////////////////////////

            string[] discountProductsPercent = new string[] { "0", "5", "10", "15", "20" };
            int[] dpValidTimeRandom = new int[] { 14, 7, 5, 3 };

            List<DiscountProduct> discountProducts = new Faker<DiscountProduct>("de")
               .CustomInstantiator(f => new DiscountProduct(Guid.NewGuid(), f.Random.ListItem(discountProductsPercent), currentTime, currentTime))
               .Generate(30)
               .ToList();
            DiscountProducts.AddRange(discountProducts);
            SaveChanges();

            ///////////////////////////////////Categorie///////////////////////////////////////

            string[] foodCategorie = new string[] { "Dairy", "Fruits", "Cereals", "Meat", "Vegetables", "Drinks" }; //Dairy = Milchprodukte, cereals = Getreide, Confections = Süßware 

            List<Categorie> categories = new Faker<Categorie>("de")
                .CustomInstantiator(f => new Categorie(Guid.NewGuid(), f.Random.ListItem(vats), string.Empty, string.Empty))
                .Rules((f, c) =>
                {
                     
                })
                .Generate(foodCategorie.Length)
                .ToList();
            Categories.AddRange(categories);
            SaveChanges();

            for (int i = 0; i < foodCategorie.Length; i++)
            {
                categories[i].Name = foodCategorie[i];
                categories[i].ShortDescription = categories[i].Name[..3];
            }
            SaveChanges();

            //////////////////////////////////Product//////////////////////////////////////

            string[,] productsCategorie = new string[,] {
                { "Milk", "Fermented milk", "Yogurt", "Cream", "Butter", "Cheese" },//, "Custard"},
                { "Apple", "Banana", "Avocado", "Cherry", "Kiwi", "Lemon"},// "Mandain"},
                { "Breads", "Porridge", "muesli", "Pasta", "noodles", "popcorn"},// "rice cakes"},
                { "Steak", "Lambmilk", "Chicken", "Pork", "Salami", "Sucuk"},// "Bacon" },
                { "Mushroom", "Brococoli", "Carrot", "Chilly", "Bean", "Potatoes"},// "Cucumber" },
                { "Coca Cola", "Fanta", "Sprite", "Pepsi", "Ayran", "Cola Zero" }// "Redbull"}
            };

            int[] price = new int[] { 3,5,8,13 };
            int count = 0;
            int countList = 0;
                List<Product> products = new Faker<Product>("de")
                    .CustomInstantiator(f => new Product(Guid.NewGuid(), f.Random.ListItem(productDetails), null, String.Empty, String.Empty, f.Random.ListItem(price), f.Random.ListItem(discountProducts).OrNull(f, 0.4f), currentTime))
                    .Rules((f, p) => {
                        //p.Price = 3;
                        p.Name = productsCategorie[count, countList];
                        p.ShortDescription = p.Name[..3];
                        p.Categorie = categories[count];
                        count++;
                        if (count == 6) {
                            count = 0;
                            countList++;
                        }
                        if (countList == 6)
                            countList = 0;
                    })
                    .Generate(36)
                    .ToList();
                    Products.AddRange(products);
                    SaveChanges();

                    int count_categorie = 0;

            //products[0].Name = productsCategorie[0, 1]; //random.Next(0, 6)];
            //products[0].ShortDescription = products[0].Name[..3];
            //products[0].Categorie = categories[0];
            //SaveChanges();

            //for (int i = 0; i < 42; i++)
            //        {
            //            if(count_categorie != 0 && count_categorie < 6)
            //                count_categorie++;
            //            if (count_categorie >= 6)
            //                 count_categorie = 0;

            //            products[i].Name = productsCategorie[count_categorie,1]; //random.Next(0, 6)];
            //            products[i].ShortDescription = products[i].Name[..3];
            //            products[i].Categorie = categories[count_categorie];
            //        }

            /////////////////////////////////DiscountCodeAlls///////////////////////////////////

            decimal[] discountCodeAllPercent = new decimal[] { 0, 2, 5, 8, 10 };

            List<DiscountCodeAll> discountCodeAlls = new Faker<DiscountCodeAll>("de")
               .CustomInstantiator(f => new DiscountCodeAll(Guid.NewGuid(), f.Internet.Password(), f.Random.ListItem(discountCodeAllPercent), currentTime, currentTime))
               .Generate(8)
               .ToList();
            DiscountCodeAlls.AddRange(discountCodeAlls);
            SaveChanges();

            /////////////////////////////////////Cart/////////////////////////////////

            List<Cart> carts = new Faker<Cart>("de")                    
               .CustomInstantiator(f => new Cart(Guid.NewGuid(), f.Random.ListItem(discountCodeAlls), f.Random.ListItem(vats),currentTime, currentTime))
               .Rules((f, cart) =>
               {
                   
                   decimal sum = 0;
                   int productLength = 0;
                   
                   cart.DiscountCodeAll = f.Random.ListItem(discountCodeAlls).OrNull(f, 0.4f);
                   
                   for (int i = 0; i < random.Next(0, 25); i++)
                   {
                       Product product = f.Random.ListItem(products);
                       cart.AddProduct(product);
                       sum += product.Price;
                       productLength += 1;
                   }
                   cart.cartPriceAll = sum;
                   cart.CountProduct = productLength;
                   
               })
               .Generate(UserLength)
               .ToList();
            Carts.AddRange(carts);
            SaveChanges();

            //////////////////////////////////////////////////////////////////////////////////

            // n..n  ich muss hier nicht addCart und addUser gleichzeitig benutzen, kann nur eine davon und er weiß bescheid, OR Mapper erledigt das

            for (int i = 0; i < UserLength; i++)
            {
                users[i].AddCart(carts[i]);
                // cart[i].AddUser(users[i]); ist nicht notwendig, wenn es eine Seite breits hat
            }
            SaveChanges();
        }
    }
}
