
using FoodShopWebsite.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Application.DiscountProducts
{
    public interface IDiscountProductService
    {
        public IEnumerable<DiscountProductDto> ListAll();
    }
}