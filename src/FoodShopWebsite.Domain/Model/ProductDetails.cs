using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Model
{
    public class ProductDetails : EntityBase
    {
        public ProductDetails() { }

        public ProductDetails(Guid guid, decimal weight, decimal kcal, decimal sugar, decimal carb)
        {
            Guid = guid;
            Weight = weight;
            Kcal = kcal;
            Sugar = sugar;
            Carb = carb;
        }

        public Guid Guid { get; set; }
        public decimal Weight  { get; set; }
        public decimal Kcal { get; set; }
        public decimal Sugar { get; set; }
        public decimal Carb { get; set; }
    }
}
