using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Model
{
    public class Product : EntityBase
    {
        public Product() { }
        public Product(Guid guid, ProductDetails productDetails, Categorie categorie,  string name, string shortDescription,
                    int price, DiscountProduct? discountProduct, DateTime changedTime)
        {
            Guid = guid;
            ProductDetails = productDetails;
            Categorie = categorie;
            Name = name;
            ShortDescription = shortDescription;
            Price = price;
            DiscountProduct = discountProduct;
            ChangedTime = changedTime;
        }

        public Guid Guid { get; set; }
        public Categorie Categorie { get; set; } = default!;
        public ProductDetails ProductDetails { get; set; } = default!;
        public string Name { get; set; } = string.Empty;    
        public string ShortDescription { get; set; } = String.Empty;
        public int Price { get; set; }
        public DiscountProduct? DiscountProduct { get; set; }
        public DateTime? ChangedTime { get; set; }

    }
}
