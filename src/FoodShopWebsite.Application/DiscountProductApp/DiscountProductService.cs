using FoodShopWebsite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodShopWebsite.Domain.Model;
using FoodShopWebsite.Dtos;
using FoodShopWebsite.Application.DiscountProducts;

namespace FoodShopWebsite.Application.DiscountProductApp
{
    public class DiscountProductService : IDiscountProductService
    {
        private readonly IRepositoryBase<DiscountProduct> _discountProductRepository;
        public DiscountProductService(IRepositoryBase<DiscountProduct> discountProductsRepository)
        {
            _discountProductRepository = discountProductsRepository;
        }
        public IEnumerable<DiscountProductDto> ListAll()
        {
            return _discountProductRepository
                   .GetAll()
                   .Select(dp => new DiscountProductDto(dp.Guid, dp.Percent, dp.ValidTimeFrom, dp.ValidTimeTo))
                   .ToList()
                   .DistinctBy(dp => dp.Percent)
                   .OrderBy(dp => dp.Percent)
                   ;
        }
    }
}
