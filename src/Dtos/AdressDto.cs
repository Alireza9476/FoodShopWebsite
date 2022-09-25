using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Dtos
{
    public class AdressDto
    {
        public string Street { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
