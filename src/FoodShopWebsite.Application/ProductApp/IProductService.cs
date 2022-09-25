using System;
using System.Linq.Expressions;
using FoodShopWebsite.Domain.Model;
using FoodShopWebsite.Dtos;

namespace FoodShopWebsite.Application.ProductApp
{
    public interface IProductService
    {
        public List<Expression<Func<Product, bool>>> FilterExpressions { get; set; }
        public Func<IQueryable<Product>, IOrderedQueryable<Product>>? SortOrderExpression { get; set; } 
        
        //In Controller wird dieser durch ein Extension Method aufgerufen
        //Durch IQueryable hat sie Funktionen wie Skip, Take, ToList die in PegenatedList benutzt werden
        public Func<IQueryable<ProductDto>, PegenatedList<ProductDto>> PagingExpression { get; set; }
        public PegenatedList<ProductDto> ListAll();

        ////////////////////////////////////////////////////////////////

        public bool Create(ProductDto dto, Guid DiscountProduct, Guid Categorie);
        public ProductDto Details(Guid id);
        public bool Edit(Guid id, int price, string? name, Guid discountProductPercent);
        public bool Delete(Guid id);
    }
}
